Bu proje, **Clerk** ile frontend'de kullanıcı kimlik doğrulaması yapıp, backend tarafında **JWT token doğrulamasıyla güvenli API iletişimini** hedeflemektedir.

## 📁 Proje Yapısı
  clerk_auth_demo/
                ├── backend/ --> .NET 8 + FastEndpoints + Clerk JWT doğrulama
                └── frontend_vite/ --> React + Vite + Clerk Frontend Kit

## 🚀 Özellikler

### ✅ Frontend (Vite + Clerk)
- Google, Apple, Facebook gibi sağlayıcılarla giriş
- Clerk kullanıcı paneli
- JWT Token otomatik olarak alınır ve backend'e gönderilir

### ✅ Backend (.NET + FastEndpoints)
- Clerk JWT token doğrulama (`Authorization: Bearer <token>`)
- `GET /api/auth/clerk-token` ile kullanıcı bilgisi çekme
- JWT token içerisindeki `sub` veya `nameidentifier` claim'i üzerinden Clerk ID ile eşleşme



## 🔧 Kurulum

### 1. Backend

 
   ` cd backend `
   ` dotnet restore `
   ` dotnet run `
Not: appsettings.json dosyasına Clerk:SecretKey eklemeyi unutmayın. Ayrıca Program.cs dosyasında da key eklemeniz gerekne bir nokta var.


### 2. Frontend
`  cd frontend_vite`
   ` npm install`
  `  npm run dev`

Not: .env dosyasına şu değişkeni eklemelisiniz: VITE_CLERK_PUBLISHABLE_KEY=pk_test_XXXX




Notlar
.env, appsettings.json, appsettings.Development.json gibi dosyalar .gitignore içerisindedir.

ClerkBackendApi ile backend tarafında Clerk user verilerine erişim sağlanır.
  
CORS ayarları frontend’in localhost’ta çalışmasına izin verir.
