# TODO — Revisão de estatísticas exibidas

Itens levantados na revisão dos labels/estatísticas exibidos (site + API), cruzando com o
plugin `l4d2_playstats.sp` (fonte da verdade dos dados).

Status: **todos os itens resolvidos.** 🟢

---

## Pendentes

_Nenhum._

---

## Concluído

### Correções de texto/significado nos 3 `.resx` (en-US, pt-BR, es-ES)

- [x] **CrownsHurt** — "Draw crowns/Draw-crowns" → "Hurt crowns" / "Crowns incompletas".
      Era erro de significado: crown com dano prévio, não *draw crown*
      (confirmado por `OnWitchCrownHurt` no plugin e pela agregação da API).
- [x] **FfHits** — "Fire-friend Hits" → "Friendly fire hits" / "Acertos de fogo amigo".
- [x] **Família Friendly Fire** padronizada — abreviações "FFG -/FFT -" → forma por extenso
      ("FF dealt -/FF taken -", "FF causado -/FF recebido -").
- [x] **SI/Commons** — hífen/maiúsculas normalizados; "Commons" (nativo) unificado em pt/es.
- [x] **es-ES: Levels** — "Niveles" → "Levels" (termo nativo).
- [x] **DeathCharges** — plural alinhado ("Instant kill" → "Instant kills" no pt).

### Decisões dos itens ⚪ (aplicadas)

- [x] **#1 — "% em SI / % no Tank": denominador trocado para acertos.**
      `HitsSi*Percent` e `HitsTank*Percent` passaram de `hits/shots` para `hits/hits`
      (fração dos acertos que foram naquele alvo).
      Arquivo: `L4D2PlayStats.Web/Views/LastMatches/Statistics.cshtml`.
- [x] **#2 — DmgTank/DmgTankIncap renomeados.** "Dano de Tank (...)" → "Dano como Tank (...)"
      (en "Damage as Tank", es "Daño como Tank"), removendo a ambiguidade "dano AO Tank".
- [x] **#3 — TankPasses renomeado.** "Perda de controle" → "Passes de Tank"
      (en "Tank passes", es "Pases de Tank").
- [x] **#4 — FF de Sniper: nenhuma ação (correto como está).**
      O plugin nunca preenche `plyFFGivenSniper`/`plyFFTakenSniper` (sempre 0; até as tabelas
      do próprio plugin omitem Sniper). Excluir da exibição está correto.
- [x] **#5 — Crowns exposto na página do jogador.**
      A API já calculava e serializava `Crowns` (completas + incompletas); o web apenas
      descartava. Adicionado:
      - `Sdk/Ranking/Results/PlayerResult.cs`: propriedade `Crowns`.
      - `Core/Players/Enums/PlayerResultProperty.cs`: membro `Crowns` (vira coluna ordenável).
      - Novo recurso `CrownsTotal` ("Total crowns" / "Crowns totais" / "Crowns totales"),
        no padrão de `SkeetsTotal`/`LevelsTotal`.
      - Exibição em `Views/Players/Details.cshtml` e `Views/Players/Index.cshtml`.
- [x] **#6 — `SiDamageDealt`: nenhuma ação.** Não é órfã — é usada na tabela por equipe em
      `Views/LastMatches/Details.cshtml`.

## Convenção de tradução

Termos nativos do jogo permanecem **sem tradução** (o qualificador pode ser traduzido):
**Levels, Skeets, Crowns, Commons, Tank, Witch, Hunter, Jockey, Charger, Smoker, Spitter, Boomer**.
Ex.: `SkeetsHurt` = "Skeets incompletos", `CrownsHurt` = "Crowns incompletas".

## Notas de verificação

- Paridade de chaves confirmada: 345 nós de dados idênticos nas 3 culturas (após `CrownsTotal`).
- XML das 3 `.resx` validado (bem-formado).
- `dotnet build` da solução web: **0 erros** (apenas warning NU1902 pré-existente do AngleSharp).
