# Blog Story

Приложение является реализацией web приложения, на базе ASP.NET Core MVC, функционал которого:
- Ведение блога (создание, обновление, удаление блогов)
- Удобный WYSIWYG редактор для создания содержимого блога, с поддержкой FileStorage (см. ниже).
- Назначение блогам тегов.
- Связывание блогов между собой (Related topics).
- Администраторское меню для управления блогами, пользователями, тэгами, хранилищем картинок.
- Пагинация блогов, поиск по блогам, полный CRUD.
- Оставлять комментарии к блогами.

## Технологии

- [.NET 6](https://dotnet.microsoft.com/en-us/download/dotnet/6.0)
- [ASP NET Core MVC](https://learn.microsoft.com/ru-ru/aspnet/core/introduction-to-aspnet-core?view=aspnetcore-7.0)
- [Docker](https://www.docker.com/)
- [CKEditor](https://ckeditor.com/)
- [Bootstrap 5](https://getbootstrap.com/)
- [EF Core 7](https://learn.microsoft.com/ru-ru/ef/core/what-is-new/ef-core-7.0/plan)
- [MediatR](https://github.com/jbogard/MediatR)
- [Swagger](https://swagger.io/)
- [AutoMapper](https://automapper.org/)
- [Postgre](https://www.postgresql.org/)

## File storage

Хранение файлов реализовано путем, загрузку файлов в монтированную папку `./storage`, с разделением на файлы и изображения.

## Запуск

`Первый вариант`, поднять инстанс `postgre`, создать БД `blog`, с владельцем `blog_user`. После чего, выполнить билдовку и запустить:

```dotnet build```

```dotnet run -lp "MyBlogOnCore"```

`Второй вариант`, использовать `docker compose`, раскомментить строку подключения к `postgre` в контейнере:

```
docker compose build
```

```
docker compose up
```

Креды пользователя сайта `admin` указаны в `appsettings.Development.json`, креды `postgres`, указаны в `docker-compose.yml`


## My instance

Запущен инстанс https://kellhus.xyz/
