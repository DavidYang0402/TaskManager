# TaskManager
一個基於 **ASP.NET Core MVC** 和 **Entity Framework Core** 的任務管理系統。

## 📌 特色
- **使用者管理**（登入/登出）
- **任務管理**（CRUD）
- **身份驗證與授權**（使用 ASP.NET Core Identity）
- **Clean Architecture 設計**

## 🚀 環境需求
- .NET 8
- MSSQL
- Visual Studio 2022

## 🔧 安裝與執行
1. **Clone 專案**
   ```bash
   git clone https://github.com/DavidYang0402/TaskManager.git
   cd TaskManager

2.**修改 ConnectionStrings**
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=TaskManager;User=root;Password=yourpassword;"
}
3.**dotnet ef database update**
4.**dotnet run**

