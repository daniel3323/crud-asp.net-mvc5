﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Data.Entity;
using System.Web;
using System.Web.Mvc;
using TesteITAU.Models;

namespace TesteITAU.Controllers
{
    public class ContaController : Controller
    {
        Random random = new Random();
        public readonly DbContexto db;

        public ContaController()
        {
            db = new DbContexto();
        }
        

        //Métodos
        [HttpGet]
        public ActionResult CriarConta()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CriarConta(Conta conta)
        {
            CriarNovaConta(conta);
            return View();
        }
        

        [HttpPost]
        public void Depositar(Lancamento lancamento, Conta conta)
        {
            DepositarValor(lancamento, conta);
        }


        [HttpGet]
        public ActionResult Sacar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Sacar(Lancamento lancamento, Conta conta)
        {
            SacarValor(lancamento, conta);
            return View();
        }


        //Functions
        private void CriarNovaConta(Conta conta)
        {

        }



        private void DepositarValor(Lancamento lancamento, Conta conta)
        {
            conta.Saldo += lancamento.Valor;
            conta.Usuario = db.Usuario.Find(Session);

            conta.Lancamentos.Add(lancamento);

            db.Entry(conta).State = EntityState.Modified;
            db.SaveChanges();
        }


        public void SacarValor(Lancamento lancamento, Conta conta)
        {            
            if(conta.Saldo > 0)
            {
                conta.Saldo -= lancamento.Valor;
                conta.Usuario = db.Usuario.Find(Session);

                conta.Lancamentos.Add(lancamento);

                db.Entry(conta).State = EntityState.Modified;
                db.SaveChanges();
            }
        }
    }
}