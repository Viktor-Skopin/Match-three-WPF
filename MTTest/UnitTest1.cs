using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Match_three_NET.Framework;

namespace MTTest
{
    [TestClass]
    public class Method_1
    {
        GameField game = new GameField(10);

        [TestMethod]
        public void Test1()
        {
            game.cells[1, 1].figure = Figure.Amethyst;
            game.cells[2, 2].figure = Figure.Citrine;

            game.SwapCells(game.cells[1, 1], game.cells[2, 2]);

            Assert.AreEqual(Figure.Citrine, game.cells[1, 1].figure);
            Assert.AreEqual(Figure.Amethyst, game.cells[2, 2].figure);
        }

        [TestMethod]
        public void Test2()
        {
            game.cells[1, 1].figure = Figure.Empty;
            game.cells[2, 2].figure = Figure.Ruby;

            game.SwapCells(game.cells[1, 1], game.cells[2, 2]);

            Assert.AreEqual(Figure.Ruby, game.cells[1, 1].figure);
            Assert.AreEqual(Figure.Empty, game.cells[2, 2].figure);
        }

        [TestMethod]
        public void Test3()
        {
            game.cells[1, 1].figure = Figure.Emerald;
            game.cells[2, 2].figure = Figure.Diamond;

            game.SwapCells(game.cells[1, 1], game.cells[2, 2]);

            Assert.AreEqual(Figure.Diamond, game.cells[1, 1].figure);
            Assert.AreEqual(Figure.Emerald, game.cells[2, 2].figure);
        }
    }

    [TestClass]
    public class Method_2
    {
        GameField game = new GameField(10);

        [TestMethod]
        public void Test1()
        {
            game.cells[1, 1].figure = Figure.Diamond;
            int result = game.DefineCellPoints(game.cells[1, 1]);

            Assert.AreEqual(30, result);
        }

        [TestMethod]
        public void Test2()
        {
            game.cells[1, 1].figure = Figure.Citrine;
            int result = game.DefineCellPoints(game.cells[1, 1]);

            Assert.AreEqual(5, result);
        }

        [TestMethod]
        public void Test3()
        {
            game.cells[1, 1].figure = Figure.Empty;
            int result = game.DefineCellPoints(game.cells[1, 1]);

            Assert.AreEqual(0, result);
        }
    }

    [TestClass]
    public class Method_3
    {
        GameField game = new GameField(10);

        [TestMethod]
        public void Test1()
        {
            game.SelectCell(1, 1);

            Assert.AreEqual(true, game.IsSomeSelected);
            Assert.AreEqual(game.cells[1,1], game.ChosenCell);
            Assert.AreEqual(true, game.cells[1, 1].IsSelected);
        }

        [TestMethod]
        public void Test2()
        {
            game.SelectCell(6, 3);

            Assert.AreEqual(true, game.IsSomeSelected);
            Assert.AreEqual(game.cells[6, 3], game.ChosenCell);
            Assert.AreEqual(true, game.cells[6, 3].IsSelected);
        }

        [TestMethod]
        public void Test3()
        {
            game.SelectCell(0, 9);

            Assert.AreEqual(true, game.IsSomeSelected);
            Assert.AreEqual(game.cells[0, 9], game.ChosenCell);
            Assert.AreEqual(true, game.cells[0, 9].IsSelected);
        }
    }

    [TestClass]
    public class Method_4
    {
        GameField game = new GameField(10);

        [TestMethod]
        public void Test1()
        {
            game.SelectCell(0, 9);
            game.UnselectCell();

            Assert.AreEqual(false, game.IsSomeSelected);
            Assert.AreEqual(null, game.ChosenCell);
            Assert.AreEqual(false, game.cells[0, 9].IsSelected);
        }

        [TestMethod]
        public void Test2()
        {
            game.SelectCell(6, 3);
            game.UnselectCell();

            Assert.AreEqual(false, game.IsSomeSelected);
            Assert.AreEqual(null, game.ChosenCell);
            Assert.AreEqual(false, game.cells[6, 3].IsSelected);
        }

        [TestMethod]
        public void Test3()
        {
            game.SelectCell(0, 0);
            game.UnselectCell();

            Assert.AreEqual(false, game.IsSomeSelected);
            Assert.AreEqual(null, game.ChosenCell);
            Assert.AreEqual(false, game.cells[0, 0].IsSelected);
        }
    }

    [TestClass]
    public class Method_5
    {
        GameField game = new GameField(10);

        [TestMethod]
        public void Test1()
        {
            bool result = game.HaveEmptyFigeres();

            Assert.AreEqual(false, result);
        }

        [TestMethod]
        public void Test2()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    game.cells[x, y].figure = Figure.Empty;
                }
            }

            bool result = game.HaveEmptyFigeres();

            Assert.AreEqual(true, result);
        }

        [TestMethod]
        public void Test3()
        {
            for (int x = 0; x < 10; x++)
            {
                for (int y = 0; y < 10; y++)
                {
                    game.cells[x, y].figure = Figure.Empty;
                }
            }

            game.cells[1, 1].figure = Figure.Amethyst;

            bool result = game.HaveEmptyFigeres();

            Assert.AreEqual(true, result);
        }
    }
}
