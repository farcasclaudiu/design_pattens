using System.Linq.Expressions;
using System.Linq;
using System.Text;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace design_patterns.behavioral.strategy
{
    /// <summary>
    /// Strategy is a behavioral design pattern that lets you 
    /// define a family of algorithms, put each of them into a separate class, 
    /// and make their objects interchangeable.
    /// 
    /// Use it when:
    /// - you want to use different variants of an algorithm 
    ///     within an object and be able to switch from one algorithm 
    ///     to another during runtime.
    /// - you have a lot of similar classes that only differ 
    ///     in the way they execute some behavior.
    /// - to isolate the business logic of a class from the 
    ///     implementation details of algorithms that may not be as important 
    ///     in the context of that logic.
    /// - your class has a massive conditional operator that switches 
    ///     between different variants of the same algorithm.
    /// </summary>
    public class StrategySample
    {
        public static async Task Run()
        {
            Console.WriteLine("Behavioral - Strategy");

            var processor = new SqlBuilderProcessor(new SqliteStrategy());
            processor.Select.AddRange(new string[]{"fieldA", "fieldB" });
            processor.From = "TableABC";
            processor.Where = new LogicExpression {
                First = new LogicExpression {
                    First = "fieldF",
                    Operator = LogicOperator.Equal,
                    Last = 1
                },
                Operator = LogicOperator.And,
                Last = true
            };
            processor.Orderby.Add(("fieldA", OrderDirection.ASC));
            processor.Orderby.Add(("fieldC", OrderDirection.DESC));
            System.Console.WriteLine(processor);

            // second strategy
            processor.SetStrategy(new MsSqlStrategy());
            System.Console.WriteLine(processor);
        }

        public interface ISqlStrategy {
            string Select(IProcessor processor);
            string From(IProcessor processor);
            string Where(IProcessor processor);
            string OrderBy(IProcessor processor);
            string GetOperatorSymbol(LogicOperator op);
        }

        public enum OrderDirection {
            ASC,
            DESC
        }

        public enum LogicOperator {
            And,
            Or,
            Not,
            Like,
            Greater,
            Less,
            Equal,
            NotEqual
        }

        public class LogicExpression
        {
            public object First { get; set; }
            public StrategySample.LogicOperator Operator { get; set; }
            public object Last { get; set; }

            private StringBuilder sb = new StringBuilder();
            private Func<LogicOperator, string> operatorFunc;
            private Func<object, string> fieldFunc;
            public string GetWhereString(Func<object, string> fieldFunc, Func<LogicOperator, string> operatorFunc) {
                sb.Clear();
                this.fieldFunc = fieldFunc;
                this.operatorFunc = operatorFunc;
                GetCondition(this);
                if(sb.Length>0)
                    sb.Insert(0, "WHERE ");
                return sb.ToString();
            }
            private void GetCondition(LogicExpression where){
                ProcessExpressionTerm(where.First);
                sb.Append($" {operatorFunc(where.Operator)} ");
                ProcessExpressionTerm(where.Last);
            }

            private void ProcessExpressionTerm(object expterm) {
                if(expterm is LogicExpression newWhere) {
                    // recursive call
                    sb.Append("(");
                    GetCondition(newWhere);
                    sb.Append(")");
                }
                else {
                    sb.Append(fieldFunc(expterm));
                }
            }
        }


        public interface IProcessor {
            void SetStrategy(ISqlStrategy strategy);

            List<string> Select { get; }
            string From { get; }
            List<(string, OrderDirection)> Orderby { get; }
            LogicExpression Where { get; }
        }

        
        public class SqlBuilderProcessor : IProcessor {
            private ISqlStrategy strategy;

            public SqlBuilderProcessor (ISqlStrategy strategy) {
                this.strategy = strategy;
            }

            public List<string> Select { get; internal set; } = new List<string>();
            public string From { get;  set; }

            public List<(string, OrderDirection)> Orderby { get; } = new List<(string, OrderDirection)>();

            // public Expression Where { get; }
            public LogicExpression Where { get; set; }

            public override string ToString()
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(strategy.Select(this));
                sb.Append(" ");
                sb.Append(strategy.From(this));
                sb.Append(" ");
                sb.Append(strategy.Where(this));
                sb.Append(" ");
                sb.Append(strategy.OrderBy(this));
                return sb.ToString();
            }

            public void SetStrategy(ISqlStrategy strategy)
            {
                this.strategy = strategy;
            }
        }

        public class SqliteStrategy : ISqlStrategy
        {
            public string From(IProcessor processor)
            {
                return $"FROM {processor.From}";
            }

            public string OrderBy(IProcessor processor)
            {
                if(processor.Orderby.Any())
                {
                    var orderby = string.Join(", ",
                    processor.Orderby.Select((f,d) => {
                        return ((OrderDirection)d == OrderDirection.ASC ? 
                        $"{f.Item1} ASC" : 
                        $"{f.Item1} DESC");
                    }).ToArray());
                    return $"ORDER BY {orderby}";
                }
                return string.Empty;
            }

            public string Select(IProcessor processor)
            {
                if(processor.Select.Any())
                    return $"SELECT {string.Join(", ", processor.Select)}";
                return "SELECT *";
            }

            // StringBuilder sb = new StringBuilder();
            public string Where(IProcessor processor)
            {
                var whereString = processor.Where.GetWhereString(f => $"{f}", l => GetOperatorSymbol(l));
                if(whereString.Length>0)
                    return $"WHERE {whereString}";
                return string.Empty;
            }

            public string GetOperatorSymbol(LogicOperator op)
            {
                switch (op)
                {
                    case LogicOperator.Equal:
                        return "=";
                    case LogicOperator.NotEqual:
                        return "<>";
                    case LogicOperator.Greater:
                        return ">";
                    case LogicOperator.Less:
                        return "<";
                    case LogicOperator.And:
                        return "AND";
                    case LogicOperator.Or:
                        return "OR";
                    default:
                        break;
                }
                return string.Empty;
            }
        }

        public class MsSqlStrategy : ISqlStrategy
        {
            public string From(IProcessor processor)
            {
                return $"FROM [{processor.From}]";
            }

            public string GetOperatorSymbol(LogicOperator op)
            {
                switch (op)
                {
                    case LogicOperator.Equal:
                        return "==";
                    case LogicOperator.NotEqual:
                        return "!=";
                    case LogicOperator.Greater:
                        return ">";
                    case LogicOperator.Less:
                        return "<";
                    case LogicOperator.And:
                        return "And";
                    case LogicOperator.Or:
                        return "Or";
                    default:
                        break;
                }
                return string.Empty;
            }

            public string OrderBy(IProcessor processor)
            {
                if(processor.Orderby.Any())
                {
                    var orderby = string.Join(", ",
                    processor.Orderby.Select((f,d) => {
                        return ((OrderDirection)d == OrderDirection.ASC ? 
                        $"[{f.Item1}] ASC" : 
                        $"[{f.Item1}] DESC");
                    }).ToArray());
                    return $"ORDER BY {orderby}";
                }
                return string.Empty;
            }

            public string Select(IProcessor processor)
            {
                if(processor.Select.Any())
                    return $"SELECT {string.Join(", ", processor.Select.Select(f => $"[{f}]"))}";
                return "SELECT *";
            }

            public string Where(IProcessor processor)
            {
                var whereString = processor.Where.GetWhereString(f => $"[{f}]", l => GetOperatorSymbol(l));
                if(whereString.Length>0)
                    return $"WHERE {whereString}";
                return string.Empty;
            }
        }
    }

    
}