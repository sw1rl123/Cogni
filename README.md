# Для упрощения процесса разработки и тестирования можно использовать докер 🐳

### Пошаговый гайд:
1) Скачать [Docker desktop](https://www.docker.com/products/docker-desktop/) 😉
2) Запустить docker desktop 🤭
3) Создать файл secrets.json с актуальными секретами в корне проекта 🔐
4) Выполнить ```docker compose -f compose.dev.yml up dev_cogni dev_chat_service``` в корне проекта 👾
5) Подождать запуска ⌛

С vpn на 4 шаг может выполниться не с первого раза при первом запуске 😥

Запуск проекта проекта происходит в watch режиме - он будет смотреть за изменениями и пересобирать при надобности 🤯 (*но он пересобирает далеко не все, например CORS в Program.cs не получится менять в рантайме*)

Запуск фронтенда для чатов: ```docker compose -f .\compose.dev.yml watch dev_chat_frontend```

Немного больше команд:
```docker compose -f compose.dev.yml up dev_cogni dev_chat_service -d``` - Запуск в фоновом режиме - так он не будет привязан к сессии консоли, в таком случае логи нужно смотреть с помощью ```docker compose -f compose.dev.yml logs -f dev_chat_service``` (именно эта команда смотрит логи только для ```dev_chat_service```, также есть ```dev_cogni```) 🫣