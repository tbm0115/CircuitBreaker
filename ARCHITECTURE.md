# CircuitBreaker multi-tenant architecture (concept)

## Goals
- Preserve visual circuit panel rendering with CSS-based styling and flexible breaker design (single, double, duplex, amperage, labels).
- Support QR-based share/scan to render a panel without embedding JSON in the QR payload (use slug to fetch server-side data).
- Enable multi-tenant operations so service companies can manage many customer panels while still allowing anonymous scan/view and self-service registration.
- Remain a Blazor .NET 8 PWA (WASM server-hosted) deployable to Azure App Service.

## Application shape
- **Front-end:** Blazor WebAssembly hosted by ASP.NET Core (interactive server-side rendering for shell + WASM for rich interactivity). PWA assets (service worker, manifest) continue to live in `wwwroot`.
- **Back-end:** ASP.NET Core host with Entity Framework Core, ASP.NET Identity, and multi-tenant domain services.
- **QR flow:** QR encodes a public panel slug URL (`/p/{slug}`). Anonymous access uses slug to read panel details (subject to `AllowAnonymousView`). No serialized JSON in the QR.

## Domain model (EF Core)
- `ApplicationUser` (Identity) – optional `DisplayName`, memberships and panel-level access.
- `Tenant` – service organizations; unique `NormalizedName`, description, created timestamp.
- `TenantMembership` – user ↔ tenant with `Role` (`Owner`, `Admin`, `Member`, `Viewer`).
- `Address` – normalized key ensures unique address identity; supports multiple panels per address.
- `Panel` – belongs to an `Address`, optional `OwnerTenant`, unique `PublicSlug`, `AllowAnonymousView`, `SlotCount`, description, access list, breakers.
- `PanelBreaker` – breaker definition per slot (`SlotIndex`, `Label`, `Amperage`, `Type`, duplex flag).
- `PanelAccess` – grants user or tenant view/edit/owner permissions per panel (owner still stored on the panel for the primary tenant).

### EF configuration highlights
- Unique indexes: `Tenant.NormalizedName`, `Address.NormalizedKey`, `Panel.PublicSlug`, `(AddressId, Name)`, `(PanelId, SlotIndex)`, `(PanelId, UserId, TenantId)` for access list, `(TenantId, UserId)` for memberships.
- Cascade deletes from `Panel` → `PanelBreaker`/`PanelAccess`, and `Address` → `Panel`.

## Services
- `IPanelService` encapsulates panel CRUD and retrieval with DTOs (`PanelSummaryDto`, `PanelDetailDto`, `BreakerDto`).
- `PanelService` handles address normalization/lookup, slug creation, breaker mapping, and anonymous-friendly retrieval by slug.

## Authentication & access
- ASP.NET Identity with cookie auth by default. Anonymous users can view public panels or register to manage their own.
- Tenants manage panels through memberships; `PanelAccess` allows inviting users or tenant-level sharing for collaboration.

## UX flow (high-level)
1. **Landing/dashboard:** quick actions to scan QR, list owned tenant panels, or create panel.
2. **Panel editor:** select panel slot count, configure breakers (type, amperage, duplex), label circuits, preview CSS-rendered panel/door.
3. **Share/QR:** generate QR that points to `/p/{slug}`; print directly via browser.
4. **Scan/view:** anonymous viewer renders the panel with labels; if authenticated and authorized, editing tools appear.

## Deployment
- Azure App Service/Web App friendly (SQLite for local/dev; swap to Azure SQL via connection string override).
- Uses service worker manifest (`ServiceWorkerAssetsManifest`) for PWA publishing.
- Static web assets include existing breaker CSS for visual fidelity; can be refined without impacting data model.

## Next steps for implementation
- Add migration and EF tooling; wire Identity auth UI or custom minimal auth components.
- Build routed pages/components: dashboard, tenant admin, panel editor/viewer, QR print/scan page.
- Introduce API endpoints (or Blazor endpoints) for anonymous panel fetch by slug and authenticated CRUD.
- Harden validation (address normalization, slot limits) and add tenant-aware authorization handlers.
