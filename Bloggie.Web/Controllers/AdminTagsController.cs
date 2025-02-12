using Bloggie.Web.Data;
using Bloggie.Web.Models.Domain;
using Bloggie.Web.Models.ViewModels;
using Bloggie.Web.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.EntityFrameworkCore;

namespace Bloggie.Web.Controllers {

    [Authorize(Roles = "SuperAdmin, Admin")]
    public class AdminTagsController : Controller {
        private readonly ITagRepository _tagRepository;

        public AdminTagsController(ITagRepository tagRepository) {
            this._tagRepository = tagRepository;
        }


        [HttpGet]
        public IActionResult Add() {
            return View();
        }

        [HttpPost]
        [ActionName("Add")]
        public async Task<IActionResult> Add(AddTagRequest addTagRequest) {
            // Mapeando AddTagRequest para Tag no dominio em model
            ValidateAddTagRequest(addTagRequest);

            if (!ModelState.IsValid) {
                return View();
            }

            var tag = new Tag {
                NameTag = addTagRequest.Name,
                DisplayNameTag = addTagRequest.DisplayName,
            };

            await _tagRepository.AddTagsAsync(tag);

            return RedirectToAction("List");
        }

        [HttpGet]
        [ActionName("List")]
        public async Task<IActionResult> List(string? searchQuery, 
            string? sortBy, 
            string? sortDirection, 
            int pageSize = 3,
            int pageNumber = 1) {

            var totalRecords = await _tagRepository.CountAsync();
            var totalPages = Math.Ceiling((decimal)totalRecords/pageSize);

            if(pageNumber > totalPages) {
                pageNumber--;
            }
            
            if(pageNumber < 1) {
                pageNumber++;
            }

            ViewBag.SearchQuery = searchQuery;
            ViewBag.SortBy = sortBy;
            ViewBag.SortDirection = sortDirection;
            ViewBag.TotalPages = totalPages;
            ViewBag.PageSize = pageSize;
            ViewBag.PageNumber = pageNumber;
            // Usa DbContext para ler o banco de dados

            var tags = await _tagRepository.GetAllTagsAsync(searchQuery, sortBy, sortDirection, pageNumber, pageSize);

            return View(tags);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(Guid id) {
            //Metodo para editar uma tag pelo id fornecido
            var tag = await _tagRepository.GetTagsAsync(id);

            if (tag != null) {
                var editTagRequest = new EditTagRequest {
                    Id = tag.Id,
                    NameTag = tag.NameTag,
                    DisplayNameTag = tag.DisplayNameTag
                };
                return View(editTagRequest);
            }

            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(EditTagRequest editTagRequest) {
            var tag = new Tag {
                Id = editTagRequest.Id,
                NameTag = editTagRequest.NameTag,
                DisplayNameTag = editTagRequest.DisplayNameTag,
            };

            var updatedTag = await _tagRepository.UpdateTagsAsync(tag);
            if (updatedTag != null) {
                //Mostra mensagem de sucesso
                return RedirectToAction("List");
            }
            else {
                //Mostra mensagem de Erro
            }


            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        [HttpPost]
        public async Task<IActionResult> Delete(EditTagRequest editTagRequest) {

            var deleteTag = await _tagRepository.DeleteTagsAsync(editTagRequest.Id);

            if (deleteTag != null) {
                //Mostra notificação de sucesso
                return RedirectToAction("List");
            }
            //Mostra Notificação de Erro
            return RedirectToAction("Edit", new { id = editTagRequest.Id });
        }

        private void ValidateAddTagRequest(AddTagRequest addTagRequest) {
            if (addTagRequest.Name != null && addTagRequest.DisplayName != null) {
                if (addTagRequest.Name.Contains(' ')) {
                    ModelState.AddModelError("Name", "Name Cannot be have spaces");
                }
            }
        }
    }
}