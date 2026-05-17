using Lib;
using System.Xml.XPath;

namespace Test
{
    [TestClass]
    public sealed class LibrarySystemTEST
    {
        private LibrarySystem _libsys = new LibrarySystem();


        [TestInitialize]
        public void start()
        {
            _libsys = new LibrarySystem();
        }

        [TestMethod]
        public void AddBookWithoutISBN_ShouldReturnFalse_BookShouldntBeAdded()
        {
            var BookWithoutISBN = new Book("test", "noISBN", "", 3026);
            var expected = _libsys.AddBook(BookWithoutISBN);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        [DataRow("Book 1", "auther 1", "9781234567897",1992)]
        [DataRow("Book 2", "auther 2", "9780123456786",1993)]
        [DataRow("Book 3", "auther 3", "9780123256786",1933)]
        [DataRow("book 4", "auther 4", "97812345167897",1942)]
        public void AddBook_ShouldReturnTrue_BookShouldExist(string title, string auther, string isbn, int publicationYear)
        {
            var book = new Book(title, auther, isbn, publicationYear);
            var result = _libsys.AddBook(book);
            Assert.IsTrue(result);
            Assert.IsNotNull(_libsys.SearchByISBN(isbn));
        }
        [TestMethod]
        public void AddBook_ShouldReturnFalse_WhenAddingSameISBN()
        {
            var book = new Book("To Kill a Mockingbird", "Harper Lee", "9780061120084", 1960);
            var result = _libsys.AddBook(book);
            Assert.IsFalse(result);
        }

        [TestMethod]
        public void RemoveBook_ShouldReturnTrue_BookShouldBeRemoved()
        {
            string isbn = "9780061120084";
            var result = _libsys.RemoveBook(isbn);
            Assert.IsTrue(result);
            Assert.IsFalse(_libsys.RemoveBook(isbn));
        }

        [TestMethod]
        public void RemovalOfLoanedBook_ShouldReturnFalse_BookCantBeRemoved()
        {
            string isbn = "9780061120084";
            var borrow = _libsys.BorrowBook(isbn);
            var expected = _libsys.RemoveBook(isbn);
            Assert.IsFalse(expected);
        }

        [TestMethod]
        public void SearchbyTitle_SearchTitleNotCaseSensetive_ShouldReturnMatchingObject()
        {
            var book = new Book("test", "Author", "1234567", 1234);
            _libsys.AddBook(book);
            var result = _libsys.SearchByTitle("TEST");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void SearchbyAuthor_SearchAuthorNotCaseSensetive_ShouldReturnMatchingObject()
        {
            var book = new Book("test", "Author", "1234567", 1234);
            _libsys.AddBook(book);
            var result = _libsys.SearchByAuthor("AUTHOR");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void SearchByTitle_SearchPartialTitleString_ShouldReturnMatchingObject()
        {
            var book = new Book("test", "Author", "1234567", 1234);
            _libsys.AddBook(book);
            var result = _libsys.SearchByTitle("tes");

            Assert.AreEqual(1, result.Count);
        }

        [TestMethod]
        public void ShowBookStatusLoaned_ShouldReturnFalse()
        {
            var book = new Book("test", "Author", "1234567", 1234);
            _libsys.AddBook(book);

            _libsys.BorrowBook("1234567");

            var result = _libsys.BorrowBook("1234567");
            Assert.IsFalse(result);


        }

        [TestMethod]
        public void LoanBook_CheckLoanDate_ShouldReturnTrue()
        {
            var book = new Book("test", "Author", "1234567", 1234);
            
            _libsys.AddBook(book);

            DateTime before = DateTime.Now;

            _libsys.BorrowBook("1234567");

            DateTime now = DateTime.Now;

            bool CheckDate = book.BorrowDate >= before && book.BorrowDate <= now;

            Assert.IsTrue(CheckDate);
        }

        [TestMethod]
        public void CalculateLateFee_ShouldReturnCorrectFee( )
        {
            var book = new Book("test", "Author", "1234567", 1234);
            _libsys.AddBook(book);

            decimal actual = _libsys.CalculateLateFee("1234567", 10);
            decimal expected = 5m;

            Assert.AreEqual(expected, actual);
        }


    }
}
