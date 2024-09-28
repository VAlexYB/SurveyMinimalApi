# SurveyMinimalAPI

API, реализующее два метода:

- Получение данных вопроса для отображения на фронте (текст вопроса и варианты ответа).
- Сохранение результатов ответа на вопрос по кнопке "Далее". Принимает выбранные радиобаттоны и возвращает ID следующего вопроса.

## Используемые технологии

- **C#** - Язык программирования
- **ASP.NET Core** - Веб-фреймворк
- **Dapper** - ORM для доступа к базе данных
- **PostgreSQL** - Система управления базами данных
- **Docker** - Контейнеризация

### Установка
1. Клонируйте репозиторий:

   ```bash
   git clone https://github.com/VAlexYB/SurveyMinimalApi.git
   cd SurveyMinimalApi
   
2. Соберите и запустите приложение с помощью Docker Compose:
   ```bash
   docker-compose up

### Эндпоинты
- GET /questions/{questionId} - Получить данные вопроса по ID.
- POST /interviews/{interviewId}/answer - Сохранить ответ на вопрос.

Примеры
- GET http://localhost:8080/questions/{questionId}
- POST http://localhost:8080/interviews/{interviewId}/answer
```bash
  Content-Type: application/json
  
  {
      "questionId": "questionId",
      "answerIds": [
          "answerId1",
          "answerId2"
      ]
  }
