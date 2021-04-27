using System.Globalization;
using System.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace design_patterns.behavioral.memento
{
    /// <summary>
    /// Memento is a behavioral design pattern that lets you 
    /// save and restore the previous state of an object 
    /// without revealing the details of its implementation.
    /// 
    /// Use it when:
    /// - you want to produce snapshots of the object’s state 
    ///     to be able to restore a previous state of the object.
    /// - direct access to the object’s fields/getters/setters 
    ///     violates its encapsulation.
    /// </summary>
    public class MementoSample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Memento");

            var board = new DrawingBoard();
            System.Console.WriteLine(board);
            // init state
            var memento0 = board.Save();

            board.DrawLine(new Line(0,0, 10, 20, 1, "red"));
            board.DrawLine(new Line(10,5, 15, 25, 2, "green"));

            System.Console.WriteLine(board);
            var memento1 = board.Save();

            board.DrawLine(new Line(20,20, 5, 20, 1, "blue"));

            System.Console.WriteLine(board);
            var memento2 = board.Save();

            board.DrawLine(new Line(7, 7, 7, 7, 3, "new red"));
            System.Console.WriteLine(board);
            var memento3 = board.Save();

#if false
            // only memento
            System.Console.WriteLine("Restore memento1");
            board.Restore(memento1);
            System.Console.WriteLine(board);
#else
            // memento with undo()
            System.Console.WriteLine("Undo one step");
            board.Undo();
            System.Console.WriteLine(board);

            System.Console.WriteLine("Undo one more step");
            board.Undo();
            System.Console.WriteLine(board);

            System.Console.WriteLine("Undo one more more step");
            board.Undo();
            System.Console.WriteLine(board);
            System.Console.WriteLine("Undo one more more more step");
            board.Undo();
            System.Console.WriteLine(board);
#endif
        }
    }

    /// <summary>
    /// keeps the snapshot of the system/component
    /// </summary>
    public class Memento {
        
        public List<Line> Lines { get; set; } = new List<Line>();

        public Memento(DrawingBoard board)
        {
            // copy inner Lines and NOT the list reference
            this.Lines.AddRange(board.Lines);
        }
    }

    public class DrawingBoard {
        public List<Line> Lines { get; set; } = new List<Line>();
        private Stack<Memento> _history = new Stack<Memento>();

        public void DrawLine(Line line)
        {
            Lines.Add(line);
        }


        public override string ToString()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendLine($"Board contains: {this.Lines.Count} lines ->");
            foreach (var line in Lines)
            {
                sb.AppendLine($"Line: from {line.fromX}-{line.fromY} to {line.toX}-{line.toY} width {line.width} color {line.color}");
            }
            return sb.ToString();
        }

        public Memento Save()
        {
            var memento = new Memento(this);
            _history.Push(memento);
            return memento;
        }

        public void Restore(Memento memento)
        {
            if (memento != null &&  
                _history.Contains(memento)) {
                while(!_history.Peek().Equals(memento)) {
                    _history.Pop();
                }
                this.Lines = memento.Lines;
            }
        }

        public void Undo() {
            if(_history.Count>1){
                _history.Pop();
                var memento = _history.Peek();
                Restore(memento);
            }
        }
    }

    public record Line(int fromX, int fromY, 
        int toX, int toY, 
        int width, string color);
}