namespace AdventOfCode.Y2024;

public sealed class Day17() : AdventDay(2024, 17)
{
    public override AdventDaySolution Solve(string input)
    {
        var lines = InputHelper.GetLines(input);

        // TODO: implement puzzle logic here

        return ("", "");
    }

    public sealed class AocProgram(params int[] args)
    {
        private readonly int[] _args = args;
        private int _instructionIndex = 0;
        
        public int RegisterA { get; set; }
        public int RegisterB { get; set; }
        public int RegisterC { get; set; }

        public int OpCode => _args[_instructionIndex];
        public int Operand => _args[_instructionIndex + 1];
        
        public List<int> Execute()
        {
            var output = new List<int>();
            
            while (_instructionIndex < _args.Length)
            {
                switch (OpCode)
                {
                    case 0:
                        Adv(Operand);
                        break;
                    case 1:
                        Bxl(Operand);
                        break;
                    case 2:
                        Bst(Operand);
                        break;
                    case 3:
                        Jnz(Operand);
                        break;
                    case 4:
                        Bxc(Operand);
                        break;
                    case 5:
                        output.Add(Out(Operand));
                        break;
                    case 6:
                        Bdv(Operand);
                        break;
                    case 7:
                        Cdv(Operand);
                        break;
                }

                _instructionIndex += 2;
            }

            return output;
        }

        public static AocProgram Parse(string input)
        {
            var lines = InputHelper.GetLines(input);

            var a = int.Parse(lines[0].Split(' ').Last());
            var b = int.Parse(lines[1].Split(' ').Last());
            var c = int.Parse(lines[2].Split(' ').Last());
            
            var args = lines.Last().Replace("Program: ", "").Split(',').Select(int.Parse).ToArray();

            return new AocProgram(args)
            {
                RegisterA = a,
                RegisterB = b,
                RegisterC = c
            };
        }

        private void Adv(int operand)
        {
            RegisterA /= (int)Math.Pow(2, Combo(operand));
        }

        private void Bst(int operand)
        {
            RegisterB = Combo(operand) % 8;
        }

        private void Bxc(int _)
        {
            Bxl(RegisterC);
        }

        private void Bxl(int operand)
        {
            RegisterB ^= operand;
        }

        private void Jnz(int operand)
        {
            if (RegisterA != 0 && _instructionIndex != operand)
                _instructionIndex = operand - 2;
        }

        private int Out(int operand)
        {
            return Combo(operand) % 8;
        }

        private void Bdv(int operand)
        {
            RegisterB = RegisterA / (int)Math.Pow(2, Combo(operand));
        }

        private void Cdv(int operand)
        {
            RegisterC = RegisterA / (int)Math.Pow(2, Combo(operand));
        }

        private int Combo(int opcode) => opcode switch
        {
            4 => RegisterA,
            5 => RegisterB,
            6 => RegisterC,
            _ => opcode
        };
    }

}
