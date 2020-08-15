using BookStore.Models;
using BookStore.Models.Dto;
using BookStore.Models.InterfaceRepo;
using BookStore.Models.Tables;
using BookStore.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AuthorBookController : Controller
    {
        private IAuthorBookRepository authorBookRepository;
        private IAuthorRepository authorRepository;
        private IBookRepository bookRepository;

        public AuthorBookController(IAuthorBookRepository authorBookRepository, IAuthorRepository authorRepository, IBookRepository bookRepository) {
            this.authorRepository = authorRepository;
            this.authorBookRepository = authorBookRepository;
            this.bookRepository = bookRepository;
        }

        [Route("Admin/AuthorBook")]
        public IActionResult DisplayAllAuthorBooks() {
            var allAuthorBooks = authorBookRepository.GetAllAuthorBooks();
            ViewBag.Title = "All author books";
            return View("Views/Admin/AuthorBook/DisplayAllAuthorBooks.cshtml", allAuthorBooks);
        }

        [HttpGet]
        [Route("Admin/AuthorBook/Create")]
        public IActionResult CreateAuthorBook()
        {
            AuthorBookViewModel viewModel = GetAuthorBookViewModel(new AuthorBook());
            return View("Views/Admin/AuthorBook/CreateAuthorBook.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/AuthorBook/Create")]
        public IActionResult CreateAuthorBook(AuthorBook authorBook)
        {
            if (ModelState.IsValid) {
                authorBookRepository.Add(authorBook);
                return RedirectToAction("DisplayAllAuthorBooks");
            }

            AuthorBookViewModel viewModel = GetAuthorBookViewModel(new AuthorBook());
            return View("Views/Admin/AuthorBook/CreateAuthorBook.cshtml", viewModel);
        }

        private AuthorBookViewModel GetAuthorBookViewModel(AuthorBook authorBook)
        {
            IEnumerable<GetAuthorDto> allAuthors = authorRepository.GetAllAuthors();
            IEnumerable<GetBookDto> allBooks = bookRepository.GetAllBooks();
            AuthorBookViewModel viewModel = new AuthorBookViewModel
            {
                AllAuthors = allAuthors,
                AllBooks = allBooks,
                AuthorBook = authorBook
            };

            return viewModel;
        }

        [HttpGet]
        [Route("Admin/AuthorBook/Edit/{id}")]
        public IActionResult EditAuthorBook(int id)
        {
            AuthorBook editAuthorBook = authorBookRepository.GetAuthorBook(id);
            if (editAuthorBook == null)
            {
                ViewBag.Object = "AuthorBook";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            AuthorBookViewModel viewModel = GetAuthorBookViewModel(editAuthorBook);
            return View("Views/Admin/AuthorBook/EditAuthorBook.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/AuthorBook/Edit/{id}")]
        public IActionResult EditAuthorBook(AuthorBook authorBook)
        {
            AuthorBook editAuthorBook = authorBookRepository.GetAuthorBook(authorBook.Id);
            if (editAuthorBook == null)
            {
                ViewBag.Object = "AuthorBook";
                return View("Views/Home/ObjectNotFound.cshtml", authorBook.Id);
            }

            if (ModelState.IsValid)
            {
                if (authorBookRepository.IsAuthorBookUnique(authorBook))
                {
                    authorBookRepository.Update(authorBook);
                    return RedirectToAction("DisplayAllAuthorBooks");
                }
                else {
                    ViewBag.Error = "Theese two items are already connected.";
                }
            }

            
            AuthorBookViewModel viewModel = GetAuthorBookViewModel(editAuthorBook);
            return View("Views/Admin/AuthorBook/EditAuthorBook.cshtml", viewModel);
        }

        [Route("Admin/AuthorBook/Delete/{id}")]
        public IActionResult DeleteAuthorBook(int id)
        {
            AuthorBook editAuthorBook = authorBookRepository.GetAuthorBook(id);
            if (editAuthorBook == null)
            {
                ViewBag.Object = "AuthorBook";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            authorBookRepository.Delete(id);
            return RedirectToAction("DisplayAllAuthorBooks");
        }

    }
}
