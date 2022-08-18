using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class ProductController : Controller
{
    private DatabaseContext _context;

    public ProductController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("/products/all")]
    public IActionResult All()
    {
        List<Product> allProducts = new List<Product>();
        allProducts = _context.Products.ToList();

        return View("All", allProducts);
    }

    [HttpGet("/products/new")]
    public IActionResult New()
    {
        return View("New");
    }

    [HttpPost("/products/create")]
    public IActionResult Create(Product newProduct)
    {
        if (ModelState.IsValid == false)
        {
            return New();
        }

        _context.Products.Add(newProduct);
        _context.SaveChanges();
        return All();
    }

    [HttpGet("/products/{productId}/details")]
    public IActionResult ShowDetails(int productId)
    {
        Product? productWithAssociations = _context.Products
        .Include(product => product.Categories)
            .ThenInclude(association => association.Category)
        .FirstOrDefault(product => product.ProductId == productId);

        // List<Product> allProducts = new List<Product>();
        // allProducts = _context.Products.ToList();
        // ViewBag.allProducts = allProducts;

        List<Category> allCategories = new List<Category>();
        allCategories = _context.Categories.ToList();
        ViewBag.allCategories = allCategories;

        if (productWithAssociations != null)
        {
            return View("Details", productWithAssociations);
        }
        return All();
    }
    


}