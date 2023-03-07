using Microsoft.AspNetCore.SignalR.Protocol;
using PawsomeProject.Server.Models;
using PawsomeProject.Shared.Models;

namespace PawsomeProject.Server.Data;

public static class DtoConversions
{
    public static IEnumerable<ProductDto> ConvertToDto(this IEnumerable<Product> products,
        IEnumerable<ProductCategory> productCategories)
    {
        return (from product in products
            join productCategory in productCategories
                on product.CategoryId equals productCategory.Id
            select new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ImageURL = product.ImageURL,
                Price = product.Price,
                Qty = product.Qty,
                CategoryId = product.CategoryId,
                CategoryName = productCategory.Name
            }).ToList();
    }

    public static ProductDto ConvertToDto(this Product product,
        ProductCategory productCategory)
    {
        var newProductDto = new ProductDto
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            ImageURL = product.ImageURL,
            Images = new List<ImageDto>(),
            Price = product.Price,
            Qty = product.Qty,
            CategoryId = product.CategoryId,
            CategoryName = productCategory.Name
        };

        foreach (var image in product.Images)
        {
            var newImageDto = new ImageDto
            {
                Id = image.Id,
                URL = image.URL,
                Type = image.Type
            };
            newProductDto.Images.Add(newImageDto);
        }

        return newProductDto;
    }

    public static IEnumerable<CartItemDto> ConvertToDto(this IEnumerable<CartItem> cartItems,
        IEnumerable<Product> products)
    {
        return (from cartItem in cartItems
            join product in products
                on cartItem.Product.Id equals product.Id
            select new CartItemDto
            {
                Id = cartItem.Id,
                ProductId = cartItem.Product.Id,
                ProductName = product.Name,
                ProductDescription = product.Description,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                Qty = cartItem.Qty,
                TotalPrice = product.Price * cartItem.Qty
            }).ToList();
    }

    public static CartItemDto ConvertToDto(this CartItem cartItem,
        Product product)
    {
        return new CartItemDto
        {
            Id = cartItem.Id,
            ProductId = cartItem.Product.Id,
            ProductName = product.Name,
            ProductDescription = product.Description,
            ProductImageURL = product.ImageURL,
            Price = product.Price,
            Qty = cartItem.Qty,
            TotalPrice = product.Price * cartItem.Qty
        };
    }

    public static OrderDto ConvertToDto(this Order order)
    {
        List<OrderItemDto> orderItems = new List<OrderItemDto>();
        foreach (var items in order.OrderItems)
        {
            Product product = new Product();
            product = items.Product;
            OrderItemDto orderItemDto = new OrderItemDto
            {
                ProductId = product.Id,
                ProductName = product.Name,
                ProductImageURL = product.ImageURL,
                Price = product.Price,
                Qty = items.OrderQuantity
            };
            orderItems.Add(orderItemDto);
        }

        return new OrderDto
        {
            Id = order.Id,
            UserEmail = order.User.Email,
            OrderItems = orderItems,
            purchasedDate = DateTime.Now
        };
    }
}