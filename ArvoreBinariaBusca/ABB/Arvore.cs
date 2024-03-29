// Erick Martins
// @erickapenas

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp3
{
    //Dados dos nos
    public class tdados
    {
        public int codigo;
        public string nome;
    }
    //tipo no
    public class nodo
    {
        public tdados info;
        public nodo esq, dir;
    }
    //// Tipo abstrato arvore ////
    public class Tarvore
    {
        nodo raiz;
        public void inicializa()
        {
            raiz = null;
        }
        public void insere(tdados x)
        {
            inserir(x, ref raiz);
        }
        void inserir(tdados x, ref nodo p)
        {
            if (p == null)
            {
                p = new nodo();
                p.info = x;
                p.esq = null;
                p.dir = null;
                return;
            }
            if (x.codigo < p.info.codigo)
            {
                inserir(x, ref p.esq);
                return;
            }
            if (x.codigo > p.info.codigo)
                inserir(x, ref p.dir);
            else
                return;
        }
        public void busca(tdados x)
        {
            pesquisa(x, raiz);
        }
        void pesquisa(tdados x, nodo p)
        {
            if (p == null)
                return;
            if (x.codigo < p.info.codigo)
            {
                pesquisa(x, p.esq);
                return;
            }
            if (x.codigo > p.info.codigo)
                pesquisa(x, p.dir);
            else
                x.nome = p.info.nome;
        }
        public void excluir(tdados x)
        {
            retira(x, ref raiz);
        }
        void antecessor(nodo q, ref nodo r)
        {
            if (r.dir != null)
            {
                antecessor(q, ref r.dir);
                return;
            }
            q.info = r.info;
            q = r;
            r = r.esq;
        }
        void retira(tdados x, ref nodo p)
        {
            nodo aux;
            if (p == null)
                return;
            if (x.codigo < p.info.codigo)
            {
                retira(x, ref p.esq);
                return;
            }
            if (x.codigo > p.info.codigo)
            {
                retira(x, ref p.dir);
                return;
            }
            if (p.dir == null)
            {
                aux = p;
                p = p.esq;                
                return;
            }
            if (p.esq != null)
            {
                antecessor(p, ref p.esq);
                return;
            }
            aux = p;
            p = p.dir;
        }
        public string listar()
        {
            string l = "";
            inordem(raiz,ref l);
            return l;
        }
        public void inordem(nodo p,ref string l)
        {
            if (p != null)
            {
                inordem(p.esq,ref  l);
                l += p.info.codigo + " - " + p.info.nome
                    +"\n";
                inordem(p.dir,ref l);
            }
        }
    }
}
