using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Entities;
using SportsStore.WebUI.Controllers;
using System.Linq;
using System.Web.Mvc;

namespace UnitTestProject1SportsStore.UnitTests
{
	[TestClass]
	public class ImageTests
	{
		[TestMethod]
		public void Can_Retrieve_Image_Data()
		{
			// Организация - создание объекта Product с данными изображения
			Product prod = new Product
			{
				ProductID = 2,
				Name = "Test",
				ImageData = new byte[] { },
				ImageMimeType = "image/png"
			};

			// Организация - создание имитированного хранилища
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { ProductID = 1, Name = "P1" },
				prod,
				new Product { ProductID = 3, Name = "P3" }
			}.AsQueryable());

			// Организация - создание контроллера
			ProductController target = new ProductController(mock.Object);

			// Действие - вызов метода действия GetImage
			ActionResult result = target.GetImage(2);

			// Утверждение
			Assert.IsNotNull(result);
			Assert.IsInstanceOfType(result, typeof(FileResult));
			Assert.AreEqual(prod.ImageMimeType, ((FileResult)result).ContentType);
		}

		[TestMethod]
		public void Cannot_Retrieve_Image_Data_For_Invalid_ID()
		{
			// Организация - создание имитированного хранилища
			Mock<IProductRepository> mock = new Mock<IProductRepository>();
			mock.Setup(m => m.Products).Returns(new Product[]
			{
				new Product { ProductID = 1, Name = "P1" },
				new Product { ProductID = 2, Name = "P2" }
			}.AsQueryable());

			// Организация - создание контроллера
			ProductController target = new ProductController(mock.Object);

			// Действие - вызов метода действия GetImage
			ActionResult result = target.GetImage(100);

			// Утверждение
			Assert.IsNull(result);
		}
	}
}
