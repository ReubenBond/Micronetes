using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Orleans;
using Shared.Contracts;

namespace FrontEnd.Pages
{
    public class IndexModel : PageModel
    {
        public void OnGet()
        {

        }

        public async Task<IActionResult> OnPost([FromServices]IGrainFactory client)
        {
            var id = Guid.NewGuid();
            var grain = client.GetGrain<IOrderGrain>(id);
            var order = new Order
            {
                OrderId = id,
                CreatedTime = DateTime.UtcNow,
                UserId = User.Identity.Name
            };

            await grain.PlaceOrderAsync(order);

            return Redirect("/");
        }
    }
}
