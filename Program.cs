using System;
using System.Collections.Generic;

namespace MatematicaComputacional_CriptografiaXor
{
    /// <summary>
    /// Programa desenvolvido em C# para Criptografia/Descriptografia, com base na Porta Lógica 'XOR'
    /// Disciplina: 'Matemática Computacional' - Professor: Luiz Gonzaga de Paulo
    /// Aluna: Ana Carolina Vernizze - RU: 251578
    /// </summary>
    public class Program
    {
        #region Constantes

        //Valor padrão a ser criptografado
        private const string ValorACriptografarDefault = "Este texto deve ser criptografado";
        //RU da Aluna
        private const string RU = "251578";

        #endregion

        #region Variáveis

        //Dicionário que contém o De/Para de caracteres ASCII para os seus respectivos valores em Decimal
        private static readonly Dictionary<char, int> _decimaisEmAscii = new Dictionary<char, int>();
        //Dicionário que contém o De/Para de valores decimais para seus respectivos caracteres ASCII
        private static readonly Dictionary<int, char> _asciiEmDecimal = new Dictionary<int, char>();

        #endregion

        #region Construtores

        static Program()
        {
            //Carga dos Dicionaários da Tabela ASCII
            for (int i = 1; i < 256; i++)
            {
                _decimaisEmAscii.Add((char)i, i);
                _asciiEmDecimal.Add(i, (char)i);
            }
        }

        #endregion

        #region Métodos

        static void Main(string[] args)
        {
            stdout("Iniciando...");
            //Criptografa a informação, retornando um Array de Char
            char[] valorCriptografado = Criptografar(ValorACriptografarDefault);
            stdout("Valor Criptografado:", valorCriptografado);
            //Descriptografa o valor previamente criptografado
            Console.WriteLine($"Valor Descriptografado: { Descriptografar(valorCriptografado) }");
            stdout("Concluindo...");
            Console.ReadLine();
        }

        public static char[] Criptografar(string valor)
        {
            char[] valorEmCharArray = new char[valor.Length];
            //Converte o parâmetro em um array de caracteres
            for (int i = 0; i < valor.Length; i++)
                valorEmCharArray[i] = valor[i];

            //Envia ao método 'Xor' para criptografia
            return Xor(valorEmCharArray);
        }

        public static string Descriptografar(char[] valor)
        {
            string resultado = string.Empty;
            //Envia ao método 'Xor' para descriptografia
            char[] valorEmCharArray = Xor(valor);
            //Converte o retorno do método 'Xor' em um valor String
            for (int i = 0; i < valorEmCharArray.Length; i++)
                resultado += valorEmCharArray[i];
            return resultado;
        }

        private static char[] Xor(char[] valor)
        {
            char[] resultado = new char[valor.Length];
            //Índice de controle do caractere do RU da Aluna
            int k = 0;
            //Itera no array de caracteres informado
            for (int i = 0; i < valor.Length; i++)
            {
                //Declara a variável que conterá o valor criptografado/descriptografado
                string valorTratado = string.Empty;
                //Busca o Valor Decimal do caractere na Tabela ASCII
                _decimaisEmAscii.TryGetValue(valor[i], out int valorDecimalAscii);
                //Converte o Valor Decimal do caractere para Binário
                string valorBinario = DecimalParaBinario(valorDecimalAscii);
                //Converte o caractere do RU da Aluna para Binário
                string valorBinarioRu = DecimalParaBinario(RU[k]);
                //Itera pelos bits do binário do caractere a ser criptografado/descriptografado
                for (int j = 0; j < valorBinario.Length; j++)
                {
                    //Aplica a Porta Lógica 'XOR' e o caractere do RU da Aluna 
                    if (valorBinario[j] == valorBinarioRu[j])
                        valorTratado += "0";
                    else
                        valorTratado += "1";
                }

                //Converte o caractere criptografado/descriptografado de Binário para Decimal
                int valorDecimalCriptografado = BinarioParaDecimal(valorTratado);
                //Busca o caractere na Tabela ASCII com base em seu respectivo Valor Decimal
                _asciiEmDecimal.TryGetValue(valorDecimalCriptografado, out char valorAscii);
                //Armazena o caractere ASCII no Array de caracteres de retorno
                resultado[i] = valorAscii;
                //Caso se esteja utilizando o último caractere, zera-se o contador e reinicia-se do primeiro
                if ((k + 1) == RU.Length)
                    k = 0;
                else
                    k++;
            }

            return resultado;
        }

        private static int BinarioParaDecimal(string numeroBinario)
        {
            //Expoente inicia-se em 0
            int expoente = 0;
            int resultado = 0;
            //Inverte-se o número binário
            string numeroInvertido = InverteString(numeroBinario);
            //Itera-se pelos bits no valor invertido
            for (int i = 0; i < numeroInvertido.Length; i++)
            {
                //Pega-se o bit e o converte no tipo Int
                int numero = Convert.ToInt32(numeroInvertido[i].ToString());
                //Soma-se ao resultado o valor do bit multiplicado por 2 elevado ao expoente
                resultado += numero * (int)Math.Pow(2, expoente);
                //Incrementa-se o expoente em 1
                expoente++;
            }
            return resultado;
        }

        private static string DecimalParaBinario(int numeroDecimal)
        {
            //Índice de controle do tamanho do resultado obtido
            int i = 0;
            string resultado = string.Empty;
            //Se o valor recebido for '0' ou '1', eles são o próprio retorno
            if (numeroDecimal == 0 || numeroDecimal == 1)
                resultado = numeroDecimal.ToString();
            else
            {
                //Caso não, itera-se enquantoo o valor informado for maior que 0
                while (numeroDecimal > 0)
                {
                    //Obtem-se o resto da divisão do Valor Decimal por 2
                    resultado += Convert.ToString(numeroDecimal % 2);
                    //Divide-se o valor Valor Decimal por 2
                    numeroDecimal = numeroDecimal / 2;
                }
                //Inverte-se o resultado para se obter o valor binário
                resultado = InverteString(resultado);
            }
            //Obtem-se o tamanho do resultado obtido
            i = resultado.Length;
            //Itera-se acrescentando-se caracteres '0' ao início do resultado, 
            //até que se complete o tamanho final de 8 dígitos
            while (i <= 8)
            {
                resultado = '0' + resultado;
                i++;
            }
            return resultado;
        }

        private static string InverteString(string valor)
        {
            string resultado = string.Empty;
            int i = valor.Length - 1;
            //Itera-se pelos caracteres do valor recebido, adicionando-os ao resultado, 
            //partir do último até o primeiro
            do
            {
                resultado += valor[i];
                i--;
            } while (i >= 0);

            return resultado;
        }

        public static void stdout(string mensagem)
            => Console.WriteLine(mensagem);

        public static void stdout(string prefixo, char[] mensagem)
        {
            var resultString = string.Empty;
            foreach (char ch in mensagem)
                resultString += ch.ToString();
            stdout(prefixo + " " + resultString);
        }

        #endregion
    }
}
