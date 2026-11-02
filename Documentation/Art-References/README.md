# Ultra-detail art pass — fase 1

Deze referenties zijn gegenereerd voor de eerste high-detail art pass van Project Veyrath. Ze zijn bedoeld als bron voor retopology, sculpting, material authoring en texture extraction; ze zijn geen vervanging voor een echte rigged 3D-mesh.

## Deliverables

| Asset | Gebruik | Bronbestand | Unity-doel |
|---|---|---|---|
| Ancient mossy stone | First Light ruïnes, wegstenen, muren | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-e98af6bb-1751-4f9c-950b-e8335bde3295.png` | `Assets/_Project/Art/Textures/ancient_mossy_stone_albedo.png` |
| Veyrath adventurer | speler-model, armor, shortblade, backpack | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-ae63c039-5e6b-436a-a1ce-77322490c464.png` | `Assets/_Project/Art/References/veyrath_adventurer_turnaround.png` |
| Edda | NPC-model, kleding, boog, gereedschap | `/Users/sylvian/.codex/generated_images/01a1981f-0c52-7bd0-90c0-cb92b4d161f9/exec-01654189-9d96-4488-a4a2-588ff11d57f5.png` | `Assets/_Project/Art/References/edda_turnaround.png` |

## Import gate

De projectmount weigert momenteel binaire writes onder `Assets/` en `Documentation/` met `Operation not permitted`. Daarom zijn de bestanden nog niet in Unity geïmporteerd. Zodra de workspace schrijfbaar is, moeten de PNG's worden gekopieerd naar de Unity-doelen en daarna worden gecontroleerd op import, texture compression, mipmaps, kleur/normal-workflow en materiaalreferenties.

## Productiestandaard

Voor echte ultra-detail runtime-assets is per hero/NPC-model nog nodig: sculpt/high-poly, retopologized game mesh, UV2, bake van normal/AO/curvature, PBR base-color/roughness/metallic/normal, LOD0–LOD2, rig, skin weights, animation set en Unity prefab. De huidige repository bevat nog primitive placeholders; deze referenties zijn de eerste stap richting die pipeline.
