using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductsAndCategories.Models;

namespace ProductsAndCategories.Controllers;

public class CategoryController : Controller
{

    private DatabaseContext _context;

    public CategoryController(DatabaseContext context)
    {
        _context = context;
    }

    [HttpGet("/categories/all")]
    public IActionResult All()
    {

        List<Category> allCategories = new List<Category>();
        allCategories = _context.Categories.ToList();

        return View("All", allCategories);

    }

    [HttpGet("/categories/new")]
    public IActionResult New()
    {
        return View("New");
    }


    [HttpPost("/categories/create")]
    public IActionResult Create(Category newCategory)
    {
        if (ModelState.IsValid)
        {
            return New();
        }
        _context.Categories.Add(newCategory);
        _context.SaveChanges();
        return All();
    }


    [HttpGet("/categories/{productId}/details")]
    public IActionResult ShowDetails(int categoryId)
    {
        var categoryWithAssociations = _context.Categories
        .Include(category => category.Products)
            .ThenInclude(association => association.Product)
        .FirstOrDefault(category => category.CategoryId == categoryId);

        List<Product> allProducts = new List<Product>();
        allProducts = _context.Products.ToList();
        ViewBag.allProducts = allProducts;
        ViewBag.categoryId = categoryId;
        
        // List<Category> allCategories = new List<Category>();
        // allCategories = _context.Categories.ToList();
        // ViewBag.allCategories = allCategories;

        if (categoryWithAssociations != null)
        {
            ViewBag.existingProducts = categoryWithAssociations.Products.ToList();
            return View("Details", categoryWithAssociations);
        }
        return All();
    }

    [HttpPost("/categories/{categoryId}/associations/add")]
    public IActionResult Associate(Association newAssociation, int categoryId)
    {
        if (ModelState.IsValid == false)
        {
            return ShowDetails(categoryId);
        }

        _context.Associations.Add(newAssociation);
        _context.SaveChanges();
        return ShowDetails(categoryId);
    }
    
}