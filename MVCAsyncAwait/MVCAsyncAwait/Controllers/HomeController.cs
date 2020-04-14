using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MVCAsyncAwait.Models;

namespace MVCAsyncAwait.Controllers
{
    public class HomeController : Controller
    {
        private readonly MovieService _movieService;

        public HomeController(MovieContext movieContext)
        {
            _movieService=new MovieService(movieContext);
        }

        public async Task<IActionResult> Index()
        {
            var movies = await _movieService.Get();
            return View(movies);
        }

        [HttpPost]
        public async Task<IActionResult> AddMovie(Movie movie)
        {
            await _movieService.Add(movie.Name);
            return RedirectToAction("Index");

        }
        
        public IActionResult AddMovie()
        {
            
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> UpdateMovie(Movie movie)
        {
            await _movieService.Update(movie);
            return RedirectToAction("Index");

        }

        public async Task<IActionResult> UpdateMovie(int? movieId)
        {
            if (!movieId.HasValue)
                return View("Index");
            var movie = await _movieService.Find(movieId.Value);
            return View(movie.FirstOrDefault());

        }

        
        public async Task<ActionResult> DeleteMovie(int? movieId)
        {
            await _movieService.Delete(movieId);
            return RedirectToAction("Index");

        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
