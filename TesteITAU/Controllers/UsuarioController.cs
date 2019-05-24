﻿using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TesteITAU.Models;

namespace TesteITAU.Controllers
{
    public class UsuarioController : Controller
    {
        public readonly DbContexto db;

        public UsuarioController()
        {
            db = new DbContexto();
        }


        //Metodos
        [HttpGet]
        public ActionResult ListarUsuarios()
        {
            ViewBag.Usuarios = db.Usuario.Where(usuario => usuario.Nome != null).ToList();
            return View();
        }       


        [HttpGet]
        public ActionResult Logar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Logar(Usuario usuario)
        {
            if(ModelState.IsValid)
            {
                using (DbContexto db = new DbContexto())
                {
                    var validaAcesso = db.Usuario.Where(u => u.Login.Equals(usuario.Login)).FirstOrDefault();

                    if (validaAcesso.Senha == usuario.Senha && validaAcesso.Login == usuario.Login)
                    {
                        Session["Nome"] = validaAcesso.Nome;
                        Session["Sobrenome"] = validaAcesso.Sobrenome;
                        Session["ID"] = validaAcesso.ID;
                        Session["UsuarioLogado"] = true;

                        return RedirectToAction("ListarUsuarios", "Usuario");
                    }
                }
            }
            else
            {
                ModelState.AddModelError("", "Login ou Senha incorretos.");
                return View();
            }

            return View();
        }


        [HttpGet]
        public ActionResult Logout()
        {
            Session.Abandon();
            Session.Clear();
            Response.Cookies.Clear();
            return RedirectToAction("Index", "Home");
        }


        [HttpGet]
        public ActionResult CadastrarUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult CadastrarUsuario(Usuario usuario)
        {
            try
            {
                CadastrarNovoUsuario(usuario);
                return RedirectToAction("Logar", "Usuario");
                //return Json(new { erro = false, msg = "Cadastrado" });
            }
            catch (Exception ex)
            {
                return Json(new { erro = true, msg = ex.Message });
            }
        }


        [HttpGet]
        public ActionResult AlterarUsuario()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AlterarUsuario(Usuario usuario)
        {
            db.Usuario.Find(Session["ID"]);
            AlterarUsuario(usuario);

            return View();
        }

        //teste
        //Functions
        private void CadastrarNovoUsuario(Usuario usuario)
        {
            db.Usuario.Add(usuario);
            db.SaveChanges();
        }

        private void AlterarUsuarioCadastrado(Usuario usuario)
        {
            db.Entry(usuario).State = EntityState.Modified;
            db.SaveChanges();
        }
    }
}