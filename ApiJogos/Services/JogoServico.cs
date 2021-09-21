using ApiJogos.Entities;
using ApiJogos.Exceptions;
using ApiJogos.InputModel;
using ApiJogos.Repositorio;
using ApiJogos.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiJogos.Services
{
    public class JogoServico : IJogoService
    {
        private readonly IJogoRepositorios _jogoRepositorios;

        public JogoServico(IJogoRepositorios jogoRepositorios)
        {
            _jogoRepositorios = jogoRepositorios;
        }
        public async Task<List<JogoViewModel>> Obter(int pagina, int quantidade)
        {
            var jogos = await _jogoRepositorios.Obter(pagina, quantidade);

            return jogos.Select(jogo => new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            }).ToList();
        }

        public async Task<JogoViewModel> Obter(Guid id)
        {
            var jogo = await _jogoRepositorios.Obter(id);

            if (jogo == null)
            {
                return null;
            }

            return new JogoViewModel
            {
                Id = jogo.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task<JogoViewModel> Inserir(JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepositorios.Obter(jogo.Nome, jogo.Produtora);

            if (entidadeJogo.Count > 0)
            {
                throw new JogoJaCadastradoException(); 
            }

            var jogoInsert = new Jogo
            {
                Id = Guid.NewGuid(),
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };

            await _jogoRepositorios.Inserir(jogoInsert);
            return new JogoViewModel
            {
                Id = jogoInsert.Id,
                Nome = jogo.Nome,
                Produtora = jogo.Produtora,
                Preco = jogo.Preco
            };
        }

        public async Task Atualizar(Guid id, JogoInputModel jogo)
        {
            var entidadeJogo = await _jogoRepositorios.Obter(id);

            if (entidadeJogo == null)
            {
                throw new JogoJaCadastradoException();
            }

            entidadeJogo.Nome = jogo.Nome;
            entidadeJogo.Produtora = jogo.Produtora;
            entidadeJogo.Preco = jogo.Preco;

            await _jogoRepositorios.Atualizar(entidadeJogo);

        }

        public async Task Atualizar(Guid id, double preco)
        {
            var entidadeJogo = await _jogoRepositorios.Obter(id);

            if (entidadeJogo == null)
            {
                throw new JogoJaCadastradoException();
            }

            entidadeJogo.Preco = preco;

            await _jogoRepositorios.Atualizar(entidadeJogo);
        }

        public async Task Remover(Guid id)
        {
            var jogo = await _jogoRepositorios.Obter(id);

            if (jogo == null)
            {
                throw new JogoJaCadastradoException();
            }
            await _jogoRepositorios.Remover(id);
        }

        public void Dispose()
        {
            _jogoRepositorios?.Dispose();
        }

    } 
}
