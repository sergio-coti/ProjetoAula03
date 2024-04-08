﻿using Newtonsoft.Json;
using ProjetoAula03.Entities;
using ProjetoAula03.Repositories;
using ProjetoAula03.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjetoAula03.Controllers
{
    /// <summary>
    /// Classe de controle para realização do fluxo de cadastro de cliente.
    /// </summary>
    public class ClienteController
    {
        /// <summary>
        /// Método para executar o passo a passo do cadastro de um cliente.
        /// </summary>
        public void CadastrarCliente()
        {
            #region Consultar os dados do endereço baseado no CEP informado

            Console.Write("INFORME O CEP.......: ");
            var cep = Console.ReadLine();

            var viaCepService = new ViaCepService();
            var data = viaCepService.GetData(cep);

            #endregion

            #region Confirmar o endereço obtido

            Console.WriteLine(data); //imprimir os dados do endereço

            Console.Write("\nCONFIRMA O ENDEREÇO OBTIDO? (S,N): ");
            var opcao = Console.ReadLine();

            if (!opcao.Equals("S", StringComparison.OrdinalIgnoreCase))
            {
                Console.Clear(); //limpar o conteúdo do console
                CadastrarCliente(); //recursividade
            }
            else
            {
                //instanciando cliente e endereço
                var cliente = new Cliente();

                //deserializando (copiando) os dados do JSON para a classe Endereco
                cliente.Endereco = JsonConvert.DeserializeObject<Endereco>(data);

                Console.Write("\nINFORME O NOME DO CLIENTE....: ");
                cliente.Nome = Console.ReadLine();

                Console.Write("\nINFORME O CPF DO CLIENTE.....: ");
                cliente.Cpf = Console.ReadLine();

                Console.Write("\nINFORME A DATA DE NASCIMENTO.: ");
                cliente.DataNascimento = DateTime.Parse(Console.ReadLine());

                Console.Write("\nNÚMERO DO ENDEREÇO...........: ");
                cliente.Endereco.Numero = Console.ReadLine();

                #region Gravando o cliente no banco de dados

                var clienteRepository = new ClienteRepository();
                clienteRepository.Inserir(cliente);

                Console.WriteLine("\nCLIENTE CADASTRADO COM SUCESSO.");

                #endregion
            }

            #endregion
        }
    }
}
