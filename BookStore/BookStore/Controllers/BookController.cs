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
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Hosting;
using System.IO;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.DataProtection;
using BookStore.Security;
using Microsoft.AspNetCore.Identity;

namespace BookStore.Controllers
{
    [Authorize(Roles = "Admin")]
    public class BookController : Controller
    {
        private IBookRepository bookRepository;
        private ICategoryRepository categoryRepository;
        private IHostEnvironment hostEnvironment;
        private IBookUserRepository bookUserRepository;
        private readonly IDataProtector dataProtector;
        private UserManager<ApplicationUser> userManager;

        public BookController(IBookRepository bookRepository,
                                IBookUserRepository bookUserRepository,
                                ICategoryRepository categoryRepository,
                                IHostEnvironment hostEnvironment,
                                IDataProtectionProvider dataProtectionProvider,
                                DataProtectionPurposeStrings dataProtectionPurposeStrings,
                                UserManager<ApplicationUser> userManager)
        {
            this.categoryRepository = categoryRepository;
            this.bookRepository = bookRepository;
            this.hostEnvironment = hostEnvironment;
            dataProtector = dataProtectionProvider.CreateProtector(dataProtectionPurposeStrings.EmployeeIdRouteValue);
            this.userManager = userManager;
            this.bookUserRepository = bookUserRepository;
        }

        [Route("Admin/Books")]
        public ViewResult DisplayAllBooks()
        {
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
            if (ModelState.IsValid)
            {

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
            if (ModelState.IsValid)
            {
                if (model.Photo != null)
                {
                    if (model.Book.PhotoName != null)
                    {
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

        public string ProcessUploadedFile(IFormFile photo)
        {
            string uniqueFileName = null;

            if (photo != null)
            {
                string uploadImageFolder = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot\\uploads\\images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(uploadImageFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(fileStream);
                }
            }

            return uniqueFileName;
        }

        [Route("Admin/Books/Delete/{id}")]
        public IActionResult DeleteBook(int id)
        {
            Book bookToDelete = bookRepository.GetBook(id);

            if (bookToDelete == null)
            {
                ViewBag.Object = "Book";
                return View("Views/Home/ObjectNotFound.cshtml", id);
            }

            if (bookToDelete.PhotoName != null)
            {
                DeleteImage(bookToDelete.PhotoName);
            }

            bookRepository.Delete(id);
            return RedirectToAction("DisplayAllBooks");
        }

        public void DeleteImage(string photoName)
        {
            string imagePathToDelete = Path.Combine(hostEnvironment.ContentRootPath, "wwwroot\\uploads\\images", photoName);
            System.IO.File.Delete(imagePathToDelete);
        }

        [HttpGet]
        [Route("Books/{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> DisplayBookDetails(string id)
        {
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

            BookUser bookUser = new BookUser
            {
                StartDate = DateTime.Today,
                EndDate = DateTime.Today,
                BookId = book.Id
            };

            if (User.Identity.IsAuthenticated)
            {
                var user = await userManager.FindByEmailAsync(User.Identity.Name);
                bookUser.UserId = user.Id;
            }
            else
            {
                bookUser.UserId = "";
            }

            DisplayBookDetailsViewModel viewModel = new DisplayBookDetailsViewModel
            {
                Book = book,
                Category = category,
                BookUser = bookUser
            };

            var getOrderdBook = bookUserRepository.Find(bookUser.BookId, bookUser.UserId);
            if (getOrderdBook == null)
            {
                viewModel.IsReserved = false;
            }
            else {
                viewModel.IsReserved = true;
            }

            return View("Views/Book/DisplayBookDetails.cshtml", viewModel);
        }

        [HttpPost]
        [Route("Books/{id}")]
        [AllowAnonymous]
        public IActionResult SetReservation(string id, BookUser bookUser)
        {
            if (ModelState.IsValid) {
                var viewModelValid = GetDisplayBookDetailsViewModel(bookUser);
                var getOrderdBook = bookUserRepository.Find(bookUser.BookId, bookUser.UserId);

                if (getOrderdBook == null)
                {
                    bookUserRepository.Add(bookUser);
                    ViewBag.Title = "Book reserved successfully";
                    ViewBag.Message = $"Book {viewModelValid.Book.Title} is successfully reserved. You can visit library to pick it up.";
                    return View("Views/Home/Message.cshtml");
                }
                else {
                    ViewBag.Title = "Already reserved book";
                    ViewBag.Message = $"Book {viewModelValid.Book.Title} is already reserved. You can visit library to pick it up.";
                    return View("Views/Home/Message.cshtml");
                }
                
                
            }

            var viewModelNotValid = GetDisplayBookDetailsViewModel(bookUser);

            return View("Views/Book/DisplayBookDetails.cshtml", viewModelNotValid);
        }

        public DisplayBookDetailsViewModel GetDisplayBookDetailsViewModel(BookUser bookUser) {
            GetBookDto book = bookRepository.GetBookDetails(bookUser.BookId);
            Category category = categoryRepository.GetCategory(book.CategoryId);
            DisplayBookDetailsViewModel viewModel = new DisplayBookDetailsViewModel
            {
                Book = book,
                Category = category,
                BookUser = bookUser
            };

            return viewModel;
        }

        [HttpGet]
        [Route("books/category")]
        [AllowAnonymous]
        public IActionResult GetBooksByCategory(int id)
        {
            IEnumerable<GetBookDto> books = bookRepository.GetBooksByCategory(id).Select(x => {
                x.EncryptedId = dataProtector.Protect(x.Id.ToString());
                return x;
            });

            return Ok(Json(books));
        }

        [HttpGet]
        [Route("books/search")]
        [AllowAnonymous]
        public IActionResult GetBooksByCategory(string search)
        {
            IEnumerable<GetBookDto> books = bookRepository.SearchBooks(search).Select(x => {
                x.EncryptedId = dataProtector.Protect(x.Id.ToString());
                return x;
            });

            return Ok(Json(books));
        }
    }
}
