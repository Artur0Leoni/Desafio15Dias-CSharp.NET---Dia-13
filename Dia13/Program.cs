using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static List<ContaBancaria> contas = new List<ContaBancaria>();
    static int proximoNumeroConta = 1001;

    static void Main()
    {
        int opcao = 0;

        while (opcao != 6)
        {
            Console.Clear();
            Console.WriteLine("BANCO DIGITAL");
            Console.WriteLine("1 - Criar Conta");
            Console.WriteLine("2 - Depositar");
            Console.WriteLine("3 - Sacar");
            Console.WriteLine("4 - Transferir");
            Console.WriteLine("5 - Listar Contas");
            Console.WriteLine("6 - Sair");
            Console.Write("Escolha uma opção: ");

            if (!int.TryParse(Console.ReadLine(), out opcao))
            {
                Console.WriteLine("Opção inválida!");
                Console.ReadLine();
                continue;
            }

            switch (opcao)
            {
                case 1:
                    CriarConta();
                    break;
                case 2:
                    Depositar();
                    break;
                case 3:
                    Sacar();
                    break;
                case 4:
                    Transferir();
                    break;
                case 5:
                    ListarContas();
                    break;
                case 6:
                    Console.WriteLine("Saindo...");
                    break;
                default:
                    Console.WriteLine("Opção inválida!");
                    Console.ReadLine();
                    break;
            }
        }
    }

    static void CriarConta()
    {
        Console.Clear();
        Console.Write("Nome do titular: ");
        string nome = Console.ReadLine();

        var conta = new ContaBancaria(proximoNumeroConta++, nome);
        contas.Add(conta);

        Console.WriteLine($"\nConta criada com sucesso! Número: {conta.Numero}");
        Console.ReadLine();
    }

    static ContaBancaria BuscarConta()
    {
        Console.Write("Digite o número da conta: ");
        int numero = int.Parse(Console.ReadLine());

        return contas.FirstOrDefault(c => c.Numero == numero);
    }

    static void Depositar()
    {
        Console.Clear();
        var conta = BuscarConta();

        if (conta == null)
        {
            Console.WriteLine("Conta não encontrada!");
            Console.ReadLine();
            return;
        }

        Console.Write("Valor para depósito: ");
        decimal valor = decimal.Parse(Console.ReadLine());

        conta.Depositar(valor);
        Console.WriteLine("Depósito realizado com sucesso!");
        Console.ReadLine();
    }

    static void Sacar()
    {
        Console.Clear();
        var conta = BuscarConta();

        if (conta == null)
        {
            Console.WriteLine("Conta não encontrada!");
            Console.ReadLine();
            return;
        }

        Console.Write("Valor para saque: ");
        decimal valor = decimal.Parse(Console.ReadLine());

        if (conta.Sacar(valor))
            Console.WriteLine("Saque realizado com sucesso!");
        else
            Console.WriteLine("Saldo insuficiente!");

        Console.ReadLine();
    }

    static void Transferir()
    {
        Console.Clear();
        Console.WriteLine("Conta ORIGEM:");
        var origem = BuscarConta();

        Console.WriteLine("Conta DESTINO:");
        var destino = BuscarConta();

        if (origem == null || destino == null)
        {
            Console.WriteLine("Conta inválida!");
            Console.ReadLine();
            return;
        }

        Console.Write("Valor da transferência: ");
        decimal valor = decimal.Parse(Console.ReadLine());

        if (origem.Transferir(destino, valor))
            Console.WriteLine("Transferência realizada!");
        else
            Console.WriteLine("Saldo insuficiente para transferência!");

        Console.ReadLine();
    }

    static void ListarContas()
    {
        Console.Clear();
        Console.WriteLine("CONTAS CADASTRADAS");

        foreach (var c in contas)
        {
            Console.WriteLine(c);
        }

        Console.ReadLine();
    }
}

// CLASSE DE DOMÍNIO

class ContaBancaria
{
    public int Numero { get; private set; }
    public string Titular { get; private set; }
    public decimal Saldo { get; private set; }

    public ContaBancaria(int numero, string titular)
    {
        Numero = numero;
        Titular = titular;
        Saldo = 0;
    }

    public void Depositar(decimal valor)
    {
        if (valor > 0)
            Saldo += valor;
    }

    public bool Sacar(decimal valor)
    {
        if (valor > 0 && valor <= Saldo)
        {
            Saldo -= valor;
            return true;
        }
        return false;
    }

    public bool Transferir(ContaBancaria destino, decimal valor)
    {
        if (Sacar(valor))
        {
            destino.Depositar(valor);
            return true;
        }
        return false;
    }

    public override string ToString()
    {
        return $"Conta: {Numero} | Titular: {Titular} | Saldo: R$ {Saldo:F2}";
    }
}