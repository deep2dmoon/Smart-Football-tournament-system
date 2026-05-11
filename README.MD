## PROJECT OVERVIEW
- SME( smart tournament Engine) is Football game tournament management system, SME is multi-tenancy system that support the organization of new football tournament/ competition, and correspondingly compute every other tounament activities , e.g Grouping Teams into Sub-groups, Build Matche Fixtures and Disqualify And Re-arrange the paricipants for the next rounds etc.

### SYSTEM ARCHITECTURE
- The design approach is centered on a monolithic architectural design , system componenents a seperated from folders to folders and tightly coupled into a siingle unit deployable codebase.


### SYSTEM COMPONENTS 
- Core Tournament Engine
     - Fixture Compononent
     - Sub-group Builder Component
     - Standings Compute component
- Background Match-Fixture Worker
- REST API Endpoints
- Authnetication and Authorisation Service

### DATA LIFECYCLE 
- Tournament is created and managed by the executives , participants and groupings and overall standings are controlled by the admin, matc fixtures jobs are executed b the background system. Tournament becomes inactive when the completed state.

### How TO USE
- Sign in as ad EXECUTIVE
- Create new tournament if tournament does not already exist
- Populate tournaments with participants
- Spread participants into sub-groups for the competition
- Builder Match fixtures with the sub-groups ( Home and Away);
- Update Tournament Standings in  (sub-group and Overall Tournament standings) with every fixture results ( using fixture ID)
- Disqualify under performings teams'
- Create new Goup for the next rounds.... till the participants are eleminated to a single participants as Winner.

## Endpoints


