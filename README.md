# OrderService

OrderService — это микросервисное приложение на .NET 7, предназначенное для обработки заказов. Состоит из двух основных компонентов: веб-API и фонового сервиса, взаимодействующих с Kafka и PostgreSQL.

## Состав

- **OrderService.API** — HTTP API для работы с заказами.
- **OrderService.Daemon** — фоновый обработчик, подписывающийся на Kafka-топики.
- **Kafka / Zookeeper** — Брокер для коммуникации микросервисов.
- **PostgreSQL** — база данных для хранения заказов.
- **Kafdrop** — UI-интерфейс для просмотра Kafka-сообщений.
- **Common**, **Tests** — общие классы и тесты.

## запуск
Для быстрого запуска потребуется Docker:
```bash
docker-compose build     # Сборка API и Daemon
docker-compose up        # Запуск всех сервисов
```

После запуска:

- API доступен на [http://localhost:5001/swagger](http://localhost:5001/swagger)
- Kafdrop доступен на [http://localhost:9000](http://localhost:9000)

## Используемые порты

| Сервис         | Порт       |
|----------------|------------|
| API            | 5001       |
| PostgreSQL     | 5432       |
| Kafka (внутр.) | 9092       |
| Kafka (лок.)   | 29092      |
| Zookeeper      | 2181       |
| Kafdrop        | 9000       |

## Kafka

Примерная схема обмена сообщениями через Kafka:

- `OrderService.API` отправляет сообщение в Kafka.
- `OrderService.Daemon` слушает и обрабатывает их. После обработки отправляет сообщение в Kafka для обработки сообщения со стороны `OrderService.API`

Kafdrop можно использовать для отладки Kafka:
[http://localhost:9000](http://localhost:9000)

## Локальный запуск

Запуск сервисов по отдельности (например, API):

```bash
cd OrderService.API
dotnet run
```

Внимание: Для корректной работы приложения локально потребуется самостоятельно развернуть Kafka, БД PostgreSQL и Kafdrop.
 Советую запускать приложение, используя Docker.