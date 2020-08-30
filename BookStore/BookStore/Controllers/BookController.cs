﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BookStore.Models.Tables;
using BookStore.Models.InterfaceRepo;
using BookStore.Models;
using BookStore.ViewModels;
using BookStore.Models.Dto;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using BookStore.Security;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private IBookRepository bookRepository;
        private ICategoryRepository categoryRepository;
        private IHostEnvironment hostEnvironment;
        private readonly IDataProtector dataProtector;

        public BookController(  IBookRepository bookRepository, 
                                ICategoryRepository categoryRepository, 
                                IHostEnvironment hostEnvironment, 
                                IDataProtectionProvider dataProtectionProvider,
                                DataProtectionPurposeStrings dataProtectionPurposeStrings) 
        {
            this.categoryRepository = categoryRepository;
            this.bookRepository = bookRepository;
            this.hostEnvironment = hostEnvironment;
            dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
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
            BookAdminFormViewModel viewModel = new BookAdminFormViewModel
            {
                AllCategories = allCategories,
                Book = new Book()
            };
            return View("Views/Admin/Book/CreateBook.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Books/Create")]
        public IActionResult CreateBook(BookAdminFormViewModel model)
        {
            if (ModelState.IsValid) {

                model.Book.PhotoName = ProcessUploadedFile(model.Photo);

                bookRepository.Add(model.Book);
                return RedirectToAction("DisplayAllBooks");
            }

            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();
            BookAdminFormViewModel viewModel = new BookAdminFormViewModel
            {
                AllCategories = allCategories,
                Book = new Book()
            };

            return View("Views/Admin/Book/CreateBook.cshtml", viewModel);
        }

        [HttpGet]
        [Route("Admin/Books/Edit/{id}")]
        public IActionResult EditBook(int id)
        {
            Book book = bookRepository.GetBook(id);
            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();

            if (book == null)
            {
                ViewBag.Object = "Book";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            BookAdminFormViewModel viewModel = new BookAdminFormViewModel
            {
                AllCategories = allCategories,
                Book = book
            };

            return View("Views/Admin/Book/EditBook.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Admin/Books/Edit/{id}")]
        public IActionResult EditBook(BookAdminFormViewModel model)
        {
            if (ModelState.IsValid) {
                if (model.Photo != null) {
                    if (model.Book.PhotoName != null) {
                        DeleteImage(model.Book.PhotoName);
                    }

                    model.Book.PhotoName = ProcessUploadedFile(model.Photo);
                }

                bookRepository.Update(model.Book);
                return RedirectToAction("DisplayAllBooks");
            }

            Book book = bookRepository.GetBook(model.Book.Id);
            IEnumerable<Category> allCategories = categoryRepository.GetAllCategories();

            if (book == null)
            {
                ViewBag.Object = "Book";
                return View("Views/Home/ObjectNotFound.cshtml", model.Book.Id);
            }

            BookAdminFormViewModel viewModel = new BookAdminFormViewModel
            {
                AllCategories = allCategories,
                Book = book
            };

            return View("Views/Admin/Book/EditBook.cshtml", viewModel);
        }

        public string ProcessUploadedFile(IFormFile photo) {
            string uniqueFileName = null;

            if (photo != null)
            {
                string uploadImageFolder = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot\\uploads\\images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadImageFolder, uniqueFileName);
                
                using (var fileStream = new FileStream(filePath, FileMode.Create)) {
                    photo.CopyTo(fileStream);
                } 
            }

            return uniqueFileName;
        }

        [Route("Admin/Books/Delete/{id}")]
        public IActionResult DeleteBook(int id) {
            Book bookToDelete = bookRepository.GetBook(id);

            if (bookToDelete == null)
            {
                ViewBag.Object = "Book";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            if (bookToDelete.PhotoName != null) {
                DeleteImage(bookToDelete.PhotoName);
            }
            
            bookRepository.Delete(id);
            return RedirectToAction("DisplayAllBooks");
        }

        public void DeleteImage(string photoName) {
            string imagePathToDelete = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot\\uploads\\images", photoName);
            System.IO.File.Delete(imagePathToDelete);
        }

        [Route("Books/{id}")]
        [AllowAnonymous]
        public IActionResult DisplayBookDetails(string id) {
            string decryptedId = dataProtector.Unprotect(id);
            int bookId = Convert.ToInt32(decryptedId);

            GetBookDto book = bookRepository.GetBookDetails(bookId);
            
            if (book == null)
            {
                ViewBag.Object = "Book";
                return View("Views/Home/ObjectNotFound.cshtml", bookId);
            }

            Category category = categoryRepository.GetCategory(book.CategoryId);

            if (category == null)
            {
                ViewBag.Object = "Category";
                return View("Views/Home/ObjectNotFound.cshtml", bookId);
            }

            DisplayBookDetailsViewModel viewModel = new DisplayBookDetailsViewModel 
            { 
                Book = book,
                Category = category
            };

            return View("Views/Book/DisplayBookDetails.cshtml", viewModel);
        }
    }
}
