using UnityEngine;

namespace Eatable {
	public interface ICardData {
		public bool Eatable { get; }
		public string ImagePath { get; }
		public string Text { get; }
	}
	public readonly struct CardData : ICardData {
		public bool Eatable { get; }
		public string ImagePath { get; }
		public string Text { get; }

		public CardData (bool eatable, string imagePath, string text) {
			Eatable = eatable;
			ImagePath = imagePath;
			Text = text;
		}
	}
	internal abstract class CardFactory {
		internal abstract ICardData CreateData ();
	}
	internal class EatableFactory : CardFactory {
		internal string [] ImagePaths { get; }
		internal string [] Texts { get; }
		internal override ICardData CreateData () {
			int rnd = Random.Range (0, ImagePaths.Length);
			return new CardData (true, ImagePaths [rnd], Texts [rnd]);
		}
		internal EatableFactory () {
			ImagePaths = new string [] { "Apricot", "Cake", "Grape", "Melon", "Raspberries", "Tomato" };
			Texts = new string [] { "Абрикос", "Кекс", "Виноград", "Дыня", "Малина", "Помидор" };
		}
	}
	internal class UneatableFactory : CardFactory {
		internal string [] ImagePaths { get; }
		internal string [] Texts { get; }
		internal override ICardData CreateData () {
			int rnd = Random.Range (0, ImagePaths.Length);
			return new CardData (false, ImagePaths [rnd], Texts [rnd]);
		}
		internal UneatableFactory () {
			ImagePaths = new string [] { "Backpack", "Ball", "Flashlight", "Lighthouse", "Rollers", "Telephone" };
			Texts = new string [] { "Рюкзак", "Мяч", "Фонарь", "Маяк", "Ролики", "Телефон" };
		}
	}
	internal class Card {
		public ICardData Data { get; }
		internal Card (CardFactory cardFactory) {
			Data = cardFactory.CreateData ();
		}
	}
}
