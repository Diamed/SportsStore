using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Abstract;
using Moq;
using SportsStore.Domain.Entities;
using System.Linq;
using SportsStore.WebUI.Controllers;
using System.Collections.Generic;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using System;
using SportsStore.WebUI.HtmlHelpers;

namespace UnitTestProject1SportsStore.UnitTests
{
	[TestClass]
	public class UnitTest1
	{
		[TestMethod]
		public void Can_Paginate()
		{
			// Организация
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { ProductID = 1, Name = "P1" },
				new Product { ProductID = 2, Name = "P2" },
				new Product { ProductID = 3, Name = "P3" },
				new Product { ProductID = 4, Name = "P4" },
				new Product { ProductID = 5, Name = "P5" }
			}.AsQueryable());
			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;
			// Действие
			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;
			// Утверждение
			Product[] prodArray = result.Products.ToArray();
			Assert.IsTrue(prodArray.Length == 2);
			Assert.AreEqual(prodArray[0].Name, "P4");
			Assert.AreEqual(prodArray[1].Name, "P5");
		}

		[TestMethod]
		public void Can_Generate_Page_Links()
		{
			// Организация - определение вспомогательного метода HTML
			// это необходимо для применения метода расширения
			HtmlHelper myHelper = null;
			// Организация - создание данных PagingInfo
			PagingInfo pagingInfo = new PagingInfo
			{
				CurrentPage = 2,
				TotalItems = 28,
				ItemsPerPage = 10
			};
			// Организация - настройка делегата с помощью лямбда-выражения
			Func<int, string> pageUrlDelegate = i => "Page" + i;
			// Действие
			MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);
			// Утверждение
			Assert.AreEqual(result.ToString(), @"<a href=""Page1"">1</a>"
											+ @"<a class=""selected"" href=""Page2"">2</a>"
											+ @"<a href=""Page3"">3</a>");
		}

		[TestMethod]
		public void Can_Send_Pagination_View_Model()
		{
			// Организация
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { ProductID = 1, Name = "P1" },
				new Product { ProductID = 2, Name = "P2" },
				new Product { ProductID = 3, Name = "P3" },
				new Product { ProductID = 4, Name = "P4" },
				new Product { ProductID = 5, Name = "P5" }
			}.AsQueryable());
			// Организация
			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;
			// Действие
			ProductsListViewModel result = (ProductsListViewModel)controller.List(null, 2).Model;
			// Утверждение
			PagingInfo pageInfo = result.PagingInfo;
			Assert.AreEqual(pageInfo.CurrentPage, 2);
			Assert.AreEqual(pageInfo.ItemsPerPage, 3);
			Assert.AreEqual(pageInfo.TotalItems, 5);
			Assert.AreEqual(pageInfo.TotalPages, 2);
		}

		[TestMethod]
		public void Can_Filter_Products()
		{
			// Организация - создание имитированного хранилища
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { ProductID = 1, Name = "P1", Category = "Cat1" },
				new Product { ProductID = 2, Name = "P2", Category = "Cat2" },
				new Product { ProductID = 3, Name = "P3", Category = "Cat1" },
				new Product { ProductID = 4, Name = "P4", Category = "Cat2" },
				new Product { ProductID = 5, Name = "P5", Category = "Cat3" }
			}.AsQueryable());
			
			// Организация - создание контроллера и установка размера
			// страницы равным трем элементам
			ProductController controller = new ProductController(mock.Object);
			controller.PageSize = 3;

			// Действие
			Product[] result = ((ProductsListViewModel)controller.List("Cat2", 1).Model).Products.ToArray();

			// Утверждение
			Assert.AreEqual(result.Length, 2);
			Assert.IsTrue(result[0].Name == "P2" && result[0].Category == "Cat2");
			Assert.IsTrue(result[1].Name == "P4" && result[1].Category == "Cat2");
		}
	}
}
