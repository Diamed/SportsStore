using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using SportsStore.WebUI.Models;
using System.Linq;
using System.Web.Mvc;

namespace UnitTestProject1SportsStore.UnitTests
{
	[TestClass]
	public class CartTests
	{
		[TestMethod]
		public void Can_Add_New_Lines()
		{
			// Организация - создание нескольких тестовых товаров
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			// Организация - создание новой корзины
			Cart target = new Cart();

			// Действие
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			CartLine[] results = target.Lines.ToArray();

			// Утверждение
			Assert.AreEqual(results.Length, 2);
			Assert.AreEqual(results[0].Product, p1);
			Assert.AreEqual(results[1].Product, p2);
		}

		[TestMethod]
		public void Can_Add_Quantity_For_Existing_Lines()
		{
			// Организация - создание нескольких тестовых товаров
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };

			// Организация - создание новой корзины
			Cart target = new Cart();

			// Действие
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			target.AddItem(p1, 10);
			CartLine[] results = target.Lines.OrderBy(c => c.Product.ProductID).ToArray();

			// Утверждение
			Assert.AreEqual(results.Length, 2);
			Assert.AreEqual(results[0].Quantity, 11);
			Assert.AreEqual(results[1].Quantity, 1);
		}

		[TestMethod]
		public void Can_Remove_Line()
		{
			// Организация - создание нескольких тестовых товаров
			Product p1 = new Product { ProductID = 1, Name = "P1" };
			Product p2 = new Product { ProductID = 2, Name = "P2" };
			Product p3= new Product { ProductID = 3, Name = "P3" };

			// Организация - создание новой корзины
			Cart target = new Cart();

			// Организация - добавление некоторых товаров в корзину
			target.AddItem(p1, 1);
			target.AddItem(p2, 3);
			target.AddItem(p3, 5);
			target.AddItem(p2, 1);

			// Действие
			target.RemoveLine(p2);

			// Утверждение
			Assert.AreEqual(target.Lines.Where(c => c.Product == p2).Count(), 0);
			Assert.AreEqual(target.Lines.Count(), 2);
		}

		[TestMethod]
		public void Calculate_Cart_Total()
		{
			// Организация - создание нескольких тестовых товаров
			Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
			Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

			// Организация - создание новой корзины
			Cart target = new Cart();

			// Действие
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);
			target.AddItem(p1, 3);
			decimal result = target.ComputeTotalValue();

			// Утверждение
			Assert.AreEqual(result, 450M);
		}

		[TestMethod]
		public void Can_Clear_Contents()
		{
			// Организация - создание нескольких тестовых товаров
			Product p1 = new Product { ProductID = 1, Name = "P1", Price = 100M };
			Product p2 = new Product { ProductID = 2, Name = "P2", Price = 50M };

			// Организация - создание новой корзины
			Cart target = new Cart();

			// Организация - добавление нескольких элементов
			target.AddItem(p1, 1);
			target.AddItem(p2, 1);

			// Действие - сброс корзины
			target.Clear();

			// Утверждение
			Assert.AreEqual(target.Lines.Count(), 0);
		}

		[TestMethod]
		public void Can_Add_To_Cart()
		{
			// Организация - создание имитированного хранилища
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1", Category = "Apples"}
			}.AsQueryable());

			// Организация - создание экземпляра Cart
			Cart cart = new Cart();

			// Организация - создание контроллера
			CartController target = new CartController(mock.Object);

			// Действие - добавление товара в корзину
			target.AddToCart(cart, 1, null);

			// Утверждение
			Assert.AreEqual(cart.Lines.Count(), 1);
			Assert.AreEqual(cart.Lines.ToArray()[0].Product.ProductID, 1);
		}

		[TestMethod]
		public void Adding_Product_To_Cart_Goes_To_Cart_Screen()
		{
			// Организация - создание имитированного хранилища
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product {ProductID = 1, Name = "P1", Category = "Apples"}
			}.AsQueryable());

			// Организация - создание экземпляра Cart
			Cart cart = new Cart();

			// Организация - создание экземпляра контроллера
			CartController target = new CartController(mock.Object);

			// Действие - добавление товара в корзину
			RedirectToRouteResult result = target.AddToCart(cart, 2, "myUrl");

			// Утверждение
			Assert.AreEqual(result.RouteValues["action"], "Index");
			Assert.AreEqual(result.RouteValues["returnUrl"], "myUrl");
		}

		[TestMethod]
		public void Can_View_Cart_Contents()
		{
			// Организация - создание экземпляра Cart
			Cart cart = new Cart();

			// Организация - создание контроллера
			CartController target = new CartController(null);

			// Действие - вызов метода действия Index
			CartIndexViewModel result = (CartIndexViewModel)target.Index(cart, "myUrl").ViewData.Model;

			// Утверждение
			Assert.AreSame(result.Cart, cart);
			Assert.AreEqual(result.ReturnUrl, "myUrl");
		}
	}
}
