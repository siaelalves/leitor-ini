using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Segurança;
using System.IO;

namespace SettingReader
{
    public class Configurações
    {
        private Segurança.Criptografia cript = 
            new Segurança.Criptografia();

        public string[] MostrarConteúdo(
            string arquivoConfig)
        {
            // Lê todo o conteúdo do arquivo config
            string sbuffer = File.ReadAllText(
                arquivoConfig,
                Encoding.Default);
            // Copia valores DESCRIPTOGRAFADOS para matriz
            //  dsbuffer
            string[] dsbuffer = cript.Descriptografar(sbuffer).Split('%');            
            // Define string de retorno
            string rbuffer = "";
            // Insere valores no string de retorno
            foreach (string j in dsbuffer)
            {
                if (j != "")
                {
                    rbuffer += j + "%";
                }
            }
            return rbuffer.Split('%');
        }

        public string[] ListarCabeçalhos (
            string arquivoConfig)
        {
            // Lê todo o conteúdo do arquivo config
            string sbuffer = File.ReadAllText(
                arquivoConfig,
                Encoding.Default);
            // Define string de retorno
            string rbuffer = "";
            // Define ponteiro da matriz para 0
            int i = 0;
            // Copia todo o conteúdo DESCRIPTOGRAFADO
            //  na matriz dsbuffer
            // Cada valor de matriz é definido com o sinal %
            string[] dsbuffer = cript.Descriptografar(sbuffer).Split('%');
            // Inicia loop de leitura da matriz dsbuffer
            do
            {
                // Se valor for vazio,
                //if (dsbuffer[i] == "")
                //{
                    // Aponta para próximo valor
                //    i += 1;
                //}
                // Se o ponteiro estiver num valor que não contém o
                //  símbolo $,
                if (dsbuffer[i].Contains("$") == false)
                {
                    // Salva o valor na matriz de retorno
                    rbuffer += dsbuffer[i] + "#";
                }
                // Aponta para próximo valor de dsbuffer
                i += 1;
                // Loop enquanto apontar para valor da matriz
                //  for menor que seu tamanho
            } while (i <= dsbuffer.Length - 1);
            // Retorna os uma matriz contendo os cabeçalhos
            return rbuffer.Split('#');
        }

        public string[] ListarEntradas(
            string arquivoConfig,
            string cabeçalho)
        {
            // Lê todo o conteúdo do arquivo config
            string sbuffer = File.ReadAllText(
                arquivoConfig,
                Encoding.Default);
            // Define string de retorno
            string rbuffer = "";
            // Define ponteiro da matriz para 0
            int i = 0;
            // Copia todo o conteúdo DESCRIPTOGRAFADO
            //  na matriz dsbuffer
            // Cada valor de matriz é definido com o sinal %
            string[] dsbuffer = cript.Descriptografar(sbuffer).Split('%');
            // Inicia loop de leitura da matriz dsbuffer
            do
            {
                // Se valor for vazio,
                //if (dsbuffer[i] == "")
                //{
                //    Aponta para próximo valor
                //    i += 1;
                //}
                // Se o ponteiro estiver num valor que não contém o
                //  símbolo $,
                if (dsbuffer[i] == cabeçalho)
                {
                    // Aponta para próximo valor
                    i += 1;
                    // Inicia loop para ler entradas
                    do
                    {
                        // Salva o valor no string de retorno
                        rbuffer += dsbuffer[i].Split('$')[0] + "#";
                        // Aponta para próxima entrada
                        i += 1;
                    } while (dsbuffer[i].Contains("$"));
                }
                // Aponta para próximo cabeçalho após
                //  última entrada do cabeçalho anterior
                i += 1;
                // Loop enquanto apontar para valor da matriz
                //  for menor que seu tamanho
            } while (i <= dsbuffer.Length - 1);
            // Retorna os uma matriz contendo os cabeçalhos
            return rbuffer.Split('#');
        }

        public void ModificarConfiguração(
            string @arquivoConfig,
            string cabeçalho,
            string entrada,
            string valor)
        {
            // Lê todo o conteúdo do arquivo config
            string sbuffer = File.ReadAllText(
                arquivoConfig,
                Encoding.Default);
            // Define ponteiro 0 para valor de matriz
            int i = 0;
            // Copia todo o conteúdo DESCRIPTOGRAFADO
            //  na matriz dsbuffer
            // Cada valor de matriz é definido com o sinal %
            string[] dsbuffer = cript.Descriptografar(sbuffer).Split('%');
            // Inicia loop de leitura da matriz dsbuffer
            do
            {
                // Se o valor i da matriz dsbuffer equivaler ao parâmetro
                //  cabeçalho,
                if (dsbuffer[i] == cabeçalho)
                {
                    // Se o ponteiro estiver antes do último valor da matriz,                    
                    if (i < dsbuffer.Length - 1) 
                    { 
                        // Ponteiro desliza para próximo vaor
                        i += 1; 
                    }
                    // Inicia loop para ler entradas
                    do
                    {
                        // Se o valor i da matriz dsbuffer equivaler ao parâmetro
                        //  entrada,
                        if (dsbuffer[i].Split('$')[0] == entrada)
                        {
                            // O valor do ponteiro é modificado após a indicação $
                            dsbuffer[i] = dsbuffer[i].Split('$')[0] + "$" + valor;
                            // Arquivo com configurações antigas é deletado.
                            File.Delete(arquivoConfig);
                            sbuffer = "";
                            // Para cada valor da matriz dsbuffer,                            
                            foreach (string l in dsbuffer)
                            {
                                // Substituir o valor CRIPTOGRAFADO em sbuffer
                                //  pelo valor DESCRIPTOGRAFADO em dsbuffer
                                sbuffer += l + "%";
                            }
                            // Reescreve-se o arquivo com o valor 
                            //  de sbuffer CRIPTOGRAFADO
                            File.WriteAllText(arquivoConfig, cript.Criptografar(
                                sbuffer), Encoding.Default);
                            return;
                        }
                        if (i < dsbuffer.Length - 1) { i += 1; }
                    } while (dsbuffer[i].Contains("$"));
                }
                if (i < dsbuffer.Length - 1) { i += 1; }
            } while (i <= dsbuffer.Length - 1);
        }

        public void AdicionarConfiguração(
            string @arquivoConfig,
            string novoCabeçalho,
            string novaEntrada,
            string valor)
        {
            string sbuffer = "";
            // Se arquivo config não existir,
            if (File.Exists(arquivoConfig) == false)
            {
                // Cria arquivo config
                File.Create(arquivoConfig);
            }
            // Mas se o arquivo já existe,
            else
            {
                // Lê todo o conteúdo do arquivo config
                sbuffer = File.ReadAllText(
                    arquivoConfig,
                    Encoding.Default);
            }
            // Copia todo o conteúdo DESCRIPTOGRAFADO
            //  na matriz dsbuffer
            // Cada valor de matriz é definido com o sinal %
            string[] dsbuffer = cript.Descriptografar(sbuffer).Split('%');
            // Prepara string de recebimento de dados
            string nbuffer = "";
            // Despeja conteúdo descriptografado em nbuffer
            foreach (string l in dsbuffer)
            {
                nbuffer += l + "%";
            }
            // Aponta para último valor da matriz dsbuffer
            int i = dsbuffer.Length - 2;
            // Insere o cabeçalho após o último valor de dsbuffer 
            //  no string nbuffer
            nbuffer += novoCabeçalho + "%";
            // Aponta para o último valor de dsbuffer
            i += 1;
            // Insere a entrada e o valor após o novo cabeçalho
            //  no string nbuffer
            nbuffer += novaEntrada + "$" + valor + "%";
            // Esvazia a matriz sbuffer
            sbuffer = "";
            // Insere os valores adicionados para sbuffer
            sbuffer = nbuffer;
            // Insere os valores no arquivo, substituindo-os
            File.WriteAllText(arquivoConfig,
                cript.Criptografar(sbuffer),
                Encoding.Default);
            // Elimina nbuffer
            nbuffer = "";
        }

        public void AdicionarEntrada(
            string @arquivoConfig,
            string cabeçalho,
            string novaEntrada,
            string valor)
        {
            string sbuffer = cript.Descriptografar(
                File.ReadAllText(
                    arquivoConfig,
                    Encoding.Default));

            string adbuffer = "";
            string[] lbuffer = sbuffer.Split('%');
            int i = 0;

            do
            {
                if (lbuffer[i] == cabeçalho)
                {
                    i += 1;
                    do
                    {
                        i += 1;
                    } while (lbuffer[i].Contains("$") == true);
                    adbuffer = novaEntrada + "$" + valor;
                }
            } while (lbuffer[i].Contains("$") == false
                && lbuffer[i] == cabeçalho);

            sbuffer = "";
            for (int j = 0; j < lbuffer.Length; j++)
            {
                if (lbuffer[j].Contains("$") == false)
                {
                    if (lbuffer[j] != cabeçalho)
                    {
                        sbuffer += adbuffer + "%";
                        sbuffer += lbuffer[j] + "%";
                    }
                    else if (lbuffer[j] == cabeçalho)
                    {
                        sbuffer += lbuffer[j] + "%";
                     }
                }
                else if (lbuffer[j].Contains("$") == true)
                {
                    sbuffer += lbuffer[j] + "%";
                }
            }
            File.WriteAllText(
                arquivoConfig,
                    cript.Criptografar(
                        sbuffer));
        }

        public string LerConfigurações(
            string @arquivoConfig,
            string cabeçalho,
            string entrada)
        {
            // Lê todo o conteúdo do arquivo config
            string sbuffer = File.ReadAllText(
                arquivoConfig,
                Encoding.Default);
            // Define ponteiro 0 para valor de matriz
            int i = 0;
            // Copia todo o conteúdo DESCRIPTOGRAFADO
            //  na matriz dsbuffer
            // Cada valor de matriz é definido com o sinal %
            string[] dsbuffer = cript.Descriptografar(sbuffer).Split('%');
            // Inicia loop de leitura dsbuffer
            do
            {
                // Se o valor i da matriz dsbuffer equivaler ao parâmetro
                //  cabeçalho,
                if (dsbuffer[i] == cabeçalho)
                {
                    // Ponteiro desliza para próximo valor
                    i += 1;                    
                    do
                    {
                        if (dsbuffer[i].Split('$')[0] == entrada)
                        {
                            return dsbuffer[i].Split('$')[1];
                        }
                        i += 1;
                    } while (dsbuffer[i].Contains("$"));
                }
                i += 1;
            } while (i <= dsbuffer.Length);
            //
            return "Cabeçalho ou entrada especificados não encontrados.";
        }
    }
}
