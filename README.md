# Система антиплагиата

Система проверки студенческих работ на предмет плагиата. Состоит из трёх микросервисов:

- **API Gateway** — единая точка входа, маршрутизация и объединение Swagger-документации.
- **FileStorageService** — загрузка файлов, хранение сдач, сохранение информации в БД.
- **FileAnalysisService** — анализ работ на плагиат, формирование и хранение отчётов.

---

## 1. Алгоритм определения признаков плагиата

Алгоритм построен на сравнении **хэш-суммы содержимого файла (SHA-256)**.

Плагиат считается обнаруженным, если:

1. Существует **более ранняя** сдача работы.
2. Она принадлежит **другому студенту**.
3. Хэш содержимого файла полностью совпадает.

В отчёте сохраняются:

- `isPlagiarism` — есть ли совпадения.
- `matchedStudentIds` — студенты, у которых найдено совпадение.
- `createdAt` — дата анализа.

---

## 2. Краткое описание архитектуры системы

Система состоит из:

### FileStorageService
- Принимает загрузку файла (`POST /upload`).
- Сохраняет файл в локальном хранилище.
- Создаёт запись сдачи (submission) в SQLite.
- После сохранения автоматически вызывает FileAnalysisService:
  ```
  POST /analyze/{submissionId}
  ```
- Предоставляет выдачу файлов и данных о сдачах.

### FileAnalysisService
- Запрашивает содержимое файла из FileStorageService.
- Хэширует содержимое (SHA-256).
- Сравнивает хэш с предыдущими сдачами.
- Создаёт отчёт и сохраняет в базе.
- Отдаёт отчёты:
    - `GET /reports/{submissionId}`
    - `GET /reports`

### API Gateway
- Принимает все запросы клиентов.
- Проксирует их к нужному сервису.
- Обеспечивает единый REST API.

---

## 3. Пользовательские сценарии работы (User Scenarios)

### 3.1. Студент загружает работу
1. Клиент отправляет запрос:
   ```
   POST /submissions
   Content-Type: multipart/form-data
   ```
2. API Gateway перенаправляет запрос в FileStorageService.
3. FileStorageService:
    - сохраняет файл
    - создаёт запись Submission в БД
    - возвращает информацию о созданной сдаче
4. FileStorageService отправляет запрос в FileAnalysisService:
   ```
   POST /reports/upload/{submissionId}
   ```
5. FileAnalysisService выполняет анализ.

---

## 4. Технические сценарии взаимодействия микросервисов

### 4.1. Загрузка файла

**Запрос от клиента:**
```
POST /submissions
Form-data:
  file: <файл>
  studentId: <идентификатор>
  taskId: <идентификатор>
```

**Gateway → FileStorageService:**
```
POST /upload
```

**FileStorageService:**
- сохраняет файл
- создаёт запись Submission
- вызывает анализ:
  ```
  POST /analyze/{submissionId}
  ```

---

### 4.2. Анализ работы

FileAnalysisService делает:

1. Запрашивает файл:
   ```
   GET /files/{submissionId}
   ```
2. Хэширует содержимое (SHA-256).
3. Ищет совпадения в базе.
4. Формирует отчёт:
   ```json
   {
     "submissionId": "...",
     "isPlagiarism": true,
     "matchedStudentIds": ["..."],
     "matchedSubmissionIds": ["..."],
     "contentHash": "...",
     "createdAt": "..."
   }
   ```
5. Сохраняет отчёт в SQLite.

---

### 4.3. Получение отчёта

**Клиент → Gateway:**
```
GET /reports/{submissionId}
```

**Gateway → FileAnalysisService:**
```
GET /reports/{submissionId}
```

**Ответ:**
```json
{
  "submissionId": "...",
  "isPlagiarism": false,
  "matchedStudentIds": [],
  "createdAt": "2025-01-12T12:15:00Z"
}
```