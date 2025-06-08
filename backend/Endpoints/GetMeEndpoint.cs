using Clerk.BackendAPI;
using FastEndpoints;
using Microsoft.AspNetCore.Authorization;

namespace backend.Endpoints
{
    [Authorize]
    public class GetMeEndpoint : EndpointWithoutRequest
    {
        private readonly ClerkBackendApi _clerk;

        public GetMeEndpoint(ClerkBackendApi clerk)     //Clerk servisini burada DI ile alıyoruz. appsettings.json'dan secret key ile oluşturulmuş Clerk API objesi geliyor.
        {
            _clerk = clerk;
        }

        public override void Configure()
        {
            Post("/api/auth/clerk-token"); // Frontend’in fetch attığı yer
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var authHeader = HttpContext.Request.Headers["Authorization"];
            Console.WriteLine("AUTH:", authHeader);

           //Burada JWT token'dan gelen sub claimini alıyoruz bu sub claimi'de clerk kullanıcı ID'dir
            var userId = User?.Identity?.Name;

            foreach (var claim in User.Claims)
            {
                Console.WriteLine($"{claim.Type} => {claim.Value}");
          
            }

            if (string.IsNullOrEmpty(userId))
            {
                await SendUnauthorizedAsync(ct);        //Sub boşsa hata 401 döndürüyoruz
                return;
            }

            var userResponse = await _clerk.Users.GetAsync(userId); //Clerk API'ya gidip userId ile kullanıcı bilgisini çekiyoruz
            var user = userResponse.User;               

            await SendAsync(new         //Front tarafına gerekli bilgileri JSON olarak gönderiyoruz.
            {
                user.Id,
                user.Username,
                user.FirstName,
                user.LastName,
                Emails = user.EmailAddresses.Select(e => e.EmailAddressValue),
                ImageUrl = user.ImageUrl
            });


        }
    }
}
