# Ultra-detail art pass — fase 1

Deze referenties zijn gegenereerd voor de eerste high-detail art pass van Project Veyrath. Ze zijn bedoeld als bron voor retopology, sculpting, material authoring en texture extraction; ze zijn geen vervanging voor een echte rigged 3D-mesh.

## Deliverables

| Asset | Gebruik | Bronbestand | Unity-doel |
|---|---|---|---|
| Ancient mossy stone | First Light ruïnes, wegstenen, muren | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-e98af6bb-1751-4f9c-950b-e8335bde3295.png` | `Assets/_Project/Art/Textures/ancient_mossy_stone_albedo.png` |
| Veyrath adventurer | speler-model, armor, shortblade, backpack | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-ae63c039-5e6b-436a-a1ce-77322490c464.png` | `Assets/_Project/Art/References/veyrath_adventurer_turnaround.png` |
| Edda | NPC-model, kleding, boog, gereedschap | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-01654189-9d96-4488-a4a2-588ff11d57f5.png` | `Assets/_Project/Art/References/edda_turnaround.png` |
| Ashen Wraith | boss-model, armor, cloak, claws, chest core | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-006723c0-1ce1-4597-8718-e77876dfe09e.png` | `Assets/_Project/Art/References/ashen_wraith_turnaround.png` |
| Weathered ashwood + iron | nederzettingen, ruïnes, props | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-4c5e87d8-2d74-4a82-a802-f219e109208b.png` | `Assets/_Project/Art/Textures/weathered_ashwood_iron_albedo.png` |
| First Light environment | regio-compositie, ruïnes, forge, road, lighting | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-92923a56-e8c3-42d3-bc10-65a7f979fa24.png` | `Assets/_Project/Art/References/first_light_environment_concept.png` |
| Ashland ground cover | terrein, foliage, lichen en losse stenen | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-85083991-e72c-4fc2-bd64-b454ba54131c.png` | `Assets/_Project/Art/Textures/ashland_groundcover_albedo.png` |
| Veyrath weapons & props | shortblade, bow, staff, hammer, heartstone, satchel, forge tools | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-68e90ee5-555a-41b3-847b-9c2985816202.png` | `Assets/_Project/Art/References/veyrath_weapons_props_sheet.png` |
| Ashen dungeon basalt | dungeon walls, arcane ruins, boss arena | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-7626cb04-53c1-4275-9ca8-46b7ccd0bd9b.png` | `Assets/_Project/Art/Textures/ashen_dungeon_basalt_albedo.png` |

## Import gate

De binaire PNG-assets zijn rechtstreeks als Git-blobs aan de publieke repository toegevoegd omdat de lokale projectmount writes onder `Assets/` weigert met `Operation not permitted`. Ze staan nu op `master` in de Unity-doelpaden hierboven. Unity moet ze nog lokaal importeren en controleren op texture compression, mipmaps, kleur/normal-workflow en materiaalreferenties zodra de projectmount opnieuw schrijfbaar is.

## Productiestandaard

Voor echte ultra-detail runtime-assets is per hero/NPC-model nog nodig: sculpt/high-poly, retopologized game mesh, UV2, bake van normal/AO/curvature, PBR base-color/roughness/metallic/normal, LOD0–LOD2, rig, skin weights, animation set en Unity prefab. De huidige repository bevat nog primitive placeholders; deze referenties zijn de eerste stap richting die pipeline.
