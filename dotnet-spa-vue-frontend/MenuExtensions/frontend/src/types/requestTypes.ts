export type WindowRequest = OpenRequest | RefreshOrCancelRequest

export type OpenRequest = {
  /** Informs host page to open new document/UI in case of creating new customer/supplier/supplier invoice */
  command: string
  /** ID of the host page document */
  primaryKey: string
}

export type RefreshOrCancelRequest = {
  /** Informs host page to refresh/reload current document/UI OR closes screen/panel where service page is loaded */
  command: string
}
