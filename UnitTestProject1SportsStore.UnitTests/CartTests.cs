using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SportsStore.Domain.Entities;
using System.Linq;

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
	}
}
