using System;
using System.Web.Mvc;
using SportsStore.Domain.Entities;

namespace SportsStore.WebUI.Binders
{
	public class CartModelBinder : IModelBinder
	{
		private const string sessionKey = "Cart";

		public object BindModel(ControllerContext controllerContext, ModelBindingContext bindingContext)
		{
			// Получить объект Cart из сеанса
			Cart cart = (Cart)controllerContext.HttpContext.Session[sessionKey];

			// Создать экземпляр Cart, если его не обнаружено в данных сервиса
			if (cart == null)
			{
				cart = new Cart();
				controllerContext.HttpContext.Session[sessionKey] = cart;
			}

			// Вернуть объект Cart
			return cart;
		}
	}
}