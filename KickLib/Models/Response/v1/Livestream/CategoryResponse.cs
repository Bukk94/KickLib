using KickLib.Models.Response.v1.Channels;
using Newtonsoft.Json;

namespace KickLib.Models.Response.v1.Livestream;

public class CategoryResponse
{
    public int Id { get; set; }

    [JsonProperty(PropertyName = "category_id")]
    public int CategoryId { get; set; }

    public string Name { get; set; }

    public string Slug { get; set; }

    public ICollection<string> Tags { get; set; }

    public string Description { get; set; }

    [JsonProperty(PropertyName = "deleted_at")]
    public DateTime? DeletedAt { get; set; }

    public int Viewers { get; set; }

    public BannerResponse Banner { get; set; }

    public ParentCategoryResponse Category { get; set; }
    /*
{
    "id": 10,
    "category_id": 1,
    "name": "Minecraft ",
    "slug": "minecraft ",
    "tags": [
      "Adventure",
      "MMO",
      "Survival",
      "Open World"
    ],
    "description": "Minecraft focuses on allowing the player to explore, interact with, and modify a dynamically-generated map made of one-cubic-meter-sized blocks. In addition to blocks, the environment features plants, mobs, and items. Some activities in the game include mining for ore, fighting hostile mobs, and crafting new blocks and tools by gathering various resources found in the game. The game's open-ended model allows players to create structures, creations, and artwork on various multiplayer servers or their single-player maps. Other features include redstone circuits for logic computations and remote actions, minecarts and tracks, and a mysterious underworld called the Nether. A designated but completely optional goal of the game is to travel to a dimension called the End, and defeat the ender dragon.",
    "deleted_at": null,
    "viewers": 491,
    "category": {
      "id": 1,
      "name": "Games",
      "slug": "games",
      "icon": "🕹️"
    }
  }
     */
}