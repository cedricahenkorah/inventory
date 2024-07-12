using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using inventory.Models;

namespace inventory.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryItemsController : ControllerBase
    {
        private readonly InventoryContext _context;

        public InventoryItemsController(InventoryContext context)
        {
            _context = context;
        }

        // GET: api/InventoryItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryItemDTO>>> GetInventoryItems()
        {
            return await _context.InventoryItems.Select(x => ItemToDTO(x)).ToListAsync();
        }

        // GET: api/InventoryItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<InventoryItemDTO>> GetInventoryItem(int id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            return ItemToDTO(inventoryItem);
        }

        // PUT: api/InventoryItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutInventoryItem(int id, InventoryItemDTO inventoryItemDTO)
        {
            if (id != inventoryItemDTO.Id)
            {
                return BadRequest();
            }

            var inventoryItem = await _context.InventoryItems.FindAsync(id);

            if (inventoryItem == null)
            {
                return NotFound();
            }

            inventoryItem.Name = inventoryItemDTO.Name;
            inventoryItem.Quantity = inventoryItemDTO.Quantity;
            inventoryItem.IsOutOfStock = inventoryItemDTO.IsOutOfStock;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!InventoryItemExists(id))
            {
                return NotFound();
            }

            return NoContent();
        }

        // POST: api/InventoryItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<InventoryItemDTO>> PostInventoryItem(InventoryItemDTO inventoryItemDTO)
        {
            var inventoryItem = new InventoryItem
            {
                Name = inventoryItemDTO.Name,
                Quantity = inventoryItemDTO.Quantity,
                IsOutOfStock = inventoryItemDTO.IsOutOfStock
            };

            _context.InventoryItems.Add(inventoryItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetInventoryItem), new { id = inventoryItem.Id }, ItemToDTO(inventoryItem));
        }

        // DELETE: api/InventoryItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryItem(int id)
        {
            var inventoryItem = await _context.InventoryItems.FindAsync(id);
            if (inventoryItem == null)
            {
                return NotFound();
            }

            _context.InventoryItems.Remove(inventoryItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryItemExists(int id)
        {
            return _context.InventoryItems.Any(e => e.Id == id);
        }

        private static InventoryItemDTO ItemToDTO(InventoryItem item) =>
            new InventoryItemDTO
            {
                Id = item.Id,
                Name = item.Name,
                Quantity = item.Quantity,
                IsOutOfStock = item.IsOutOfStock
            };
    }
}
