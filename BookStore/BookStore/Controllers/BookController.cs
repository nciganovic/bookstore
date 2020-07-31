using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models.Tables;
using BookStore.Models.InterfaceRepo;
using BookStore.Models;
using BookStore.ViewModels;
using BookStore.Models.Dto;

namespace BookStore.Controllers
{
    public class BookController : Controller
    {
        private IBookRepository bookRepository;
        private ICategoryRepository categoryRepository;

        public BookController(IBookRepository bookRepository, ICategoryRepository categoryRepository) {
            this.categoryRepository = categoryRepository;
            this.bookRepository = bookRepository;
        }

        [Route("Admin/Books")]
        public ViewResult DisplayAllBooks() {
            IEnumerable<GetBookDto> allBooks = bookRepository.GetAllBooks();
            ViewBag.Title = "All books";
            return View("Views/Admin/Book/DisplayAllBooks.cshtml", allBooks);
        }

        [HttpGet]
        [Route("Admin/Books/Create")]
        public ViewResult CreateBook()
        {
            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();
            CreateBookViewModel viewModel = new CreateBookViewModel
            {
                AllCategories = allCategories,
                Book = new Book()
            };
            return View("Views/Admin/Book/CreateBook.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Books/Create")]
        public IActionResult CreateBook(Book book)
        {
            if (ModelState.IsValid) {
                bookRepository.Add(book);
                return RedirectToAction("DisplayAllBooks");
            }

            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();
            CreateBookViewModel viewModel = new CreateBookViewModel {
                AllCategories = allCategories,
                Book = new Book()
            };

            return View("Views/Admin/Book/CreateBook.cshtml", viewModel);
        }

        [HttpGet]
        [Route("Admin/Books/Edit/{id}")]
        public IActionResult EditBook(int id)
        {
            Book getBookById = bookRepository.GetBook(id);
            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();

            EditBookViewModel viewModel = new EditBookViewModel
            {
                AllCategories = allCategories,
                Book = getBookById
            };

            return View("Views/Admin/Book/EditBook.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Books/Edit/{id}")]
        public IActionResult EditBook(Book book)
        {
            if (ModelState.IsValid) {
                bookRepository.Update(book);
                return RedirectToAction("DisplayAllBooks");
            }

            Book getBookById = bookRepository.GetBook(book.Id);
            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();

            EditBookViewModel viewModel = new EditBookViewModel
            {
                AllCategories = allCategories,
                Book = getBookById
            };

            return View("Views/Admin/Book/EditBook.cshtml", viewModel);
        }

        [Route("Admin/Books/Delete/{id}")]
        public IActionResult DeleteBook(int id) {
            bookRepository.Delete(id);
            return RedirectToAction("DisplayAllBooks");
        }
    }
}
