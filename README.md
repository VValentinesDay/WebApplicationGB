Для работы с MySql необходим фреймворк Pomelo (Pomelo.EntityFrameworkCore.MySql).
Неожиданно-важным оказалась последовательность параметров в строке соединения, в моём случае это:
string connectionString = "Server=localhost; Port=3306; Database=test; Uid=root; Pwd=22022202;";

После завершения создания сущностей создаётся миграция 
dotnet ef migrations add InitialCreate

Кончено же всё заработает не с первого раза. В моём случае пришлось доустановить ef design и пересобрать приложение.
После миграции БД необходимо обновить 
dotnet ef database update

Опять таки, здесь пришлось повозиться. Помогола удаление и создание новой миграции:
dotnet ef migrations remove
И затем ещё раз:
dotnet ef migrations add InitialCreate

После этого БД обновились и связались с сервером
