using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MVCAsyncAwait.Models;

namespace MVCAsyncAwait
{
    public class MovieService
    {
        private MovieContext _context;

        public MovieService(MovieContext context)
        {
            _context = context;
        }

        public async Task Add(string name)
        {
            var blog = new Movie { Name = name };
            await _context.Movies.AddAsync(blog);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Movie>> Find(string name)
        {
            return await _context.Movies
                .Where(b => b.Name == name)
                .OrderBy(b => b.Name).ToListAsync();
        }
        public async Task<IEnumerable<Movie>> Find(int id)
        {
            return await _context.Movies
                .Where(b => b.MovieId == id)
                .OrderBy(b => b.Name).ToListAsync();
        }


        public async Task<IEnumerable<Movie>> Get()
        {
            return await _context.Movies.ToListAsync();
        }

        public async Task Update(Movie movie)
        {
            var movieToUpdate = await _context.Movies.FirstOrDefaultAsync(x => x.MovieId == movie.MovieId);
            movieToUpdate.Name = movie.Name;
            await _context.SaveChangesAsync();
        }

        public async Task Delete(int? movieId)
        {
            var movieToDelete = await _context.Movies.FirstOrDefaultAsync(x => x.MovieId == movieId);
            _context.Movies.Attach(movieToDelete);
            _context.Movies.Remove(movieToDelete);
            await _context.SaveChangesAsync();
        }
    }
}
