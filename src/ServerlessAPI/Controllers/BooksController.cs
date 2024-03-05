/*--****************************************************************************
 --* Project Name    : aws-sam-dotnet-serverless-api-with-dynamodb
 --* Reference       : Microsoft.AspNetCore.Mvc
 --*                   ServerlessAPI.Domain
 --* Description     : Items controller
 --* Configuration Record
 --* Review            Ver  Author           Date      Cr       Comments
 --* 001               001  A HATKAR         15/11/23  CR-XXXXX Original
 --****************************************************************************/
using Microsoft.AspNetCore.Mvc;
using ServerlessAPI.Domain;

namespace ServerlessAPI.Controllers;

/// <summary>
/// Represents a book api controller
/// </summary>
[Route("api/v1/[controller]")]
public partial class BooksController : BaseApiController
{
    #region Fields

    private readonly ILogger<BooksController> _logger;
    private readonly IRepository<Book> _bookRepository;

    #endregion

    #region Ctor

    public BooksController(ILogger<BooksController> logger,
        IRepository<Book> bookRepository)
    {
        _logger = logger;
        _bookRepository = bookRepository;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Get list of book items
    /// </summary>
    /// <param name="limit"></param>
    /// <returns>List of books</returns>
    // GET: api/v1/books/List
    [HttpGet]
    [Route("List")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<IEnumerable<Book>>> ListAsync([FromQuery] int limit = 10)
    {
        if (limit <= 0 || limit > 100) return BadRequest("The limit should been between [1-100]");

        return Ok(await _bookRepository.GetBooksAsync(limit));
    }

    /// <summary>
    /// Gets a book
    /// </summary>
    /// <param name="id">Book identifier</param>
    /// <returns>Book</returns>
    // GET: api/v1/books/GetById/5
    [HttpGet]
    [Route("GetById/{id}")]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<Book>> GetBookByIdAsync(Guid id)
    {
        // try to get book with the specified id
        var result = await _bookRepository.GetByIdAsync(id);

        if (result == null)
            return NotFound();

        return Ok(result);
    }

    /// <summary>
    /// Create a new book
    /// </summary>
    /// <param name="book">Book dto model</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    // POST: api/v1/books/Create
    [HttpPost]
    [Route("Create")]
    [ProducesResponseType(typeof(Book), StatusCodes.Status200OK)]
    public virtual async Task<ActionResult<Book>> CreateAsync([FromBody] Book book)
    {
        if (book == null) return ValidationProblem("Invalid input! Book not informed");

        var result = await _bookRepository.CreateAsync(book);

        if (result)
        {
            return CreatedAtAction(nameof(GetBookByIdAsync), new { id = book.Id }, book);
        }
        else
        {
            return BadRequest("Fail to persist");
        }
    }

    /// <summary>
    /// Update book
    /// </summary>
    /// <param name="id">Book identifier</param>
    /// <param name="book">Book dto model</param>
    /// <returns>A task that represents the asychronous operation</returns>
    // PUT: api/v1/books/Update/5
    [HttpPut]
    [Route("Update/{id}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public virtual async Task<IActionResult> UpdateAsync(Guid id, [FromBody] Book book)
    {
        if (id == Guid.Empty || book == null) return ValidationProblem("Invalid request payload");

        // try to get book with the specified id
        var bookRetrieved = await _bookRepository.GetByIdAsync(id);

        if (bookRetrieved == null)
        {
            var errorMsg = $"Invalid input! No book found with id:{id}";
            return NotFound(errorMsg);
        }

        book.Id = bookRetrieved.Id;

        await _bookRepository.UpdateAsync(book);

        return Ok();
    }

    /// <summary>
    /// Delete a book
    /// </summary>
    /// <param name="id">Book identifier</param>
    /// <returns>A task that represents the asynchronous operation</returns>
    // DELETE: api/v1/books/Delete/5
    [HttpDelete]
    [Route("Delete/{id}")]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public virtual async Task<IActionResult> Delete(Guid id)
    {
        if (id == Guid.Empty) return ValidationProblem("Invalid request payload");

        //try to get book with the specified id
        var bookRetrieved = await _bookRepository.GetByIdAsync(id);

        if (bookRetrieved == null)
        {
            var errorMsg = $"Invalid input! No book found with id:{id}";
            return NotFound(errorMsg);
        }

        await _bookRepository.DeleteAsync(bookRetrieved);

        return Ok();
    }

    #endregion
}