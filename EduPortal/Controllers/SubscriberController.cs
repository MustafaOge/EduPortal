using EduPortal.Domain.Entities;
using EduPortal.Persistence.context;
using Microsoft.AspNetCore.Mvc;

namespace EduPortal.Controllers
{
    public class SubscriberController(AppDbContext dbContext) : Controller
    {
        [HttpPost]
        public IActionResult SubscriberCreate(Subscriber subscriber)
        {
            if (ModelState.IsValid)
            {
                dbContext.Subscribers.Add(subscriber);
                dbContext.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(subscriber);
        }
        [Route("Subscriber")]
        public IActionResult SubscriberCreate()
        {
            return View();
        }
        public IActionResult SubscriptionRemove()
        {
            return View();
        }
    }
}
