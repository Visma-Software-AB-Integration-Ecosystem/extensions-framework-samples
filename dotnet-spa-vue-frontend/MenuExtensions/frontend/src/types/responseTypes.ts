export type InitMessage = {
  /** Informs page to do initialization based on currently loaded document for current company/user in host page */
  command: string
  /** URL of ERP host page (the page hosting the IFRAME) - to be used as message target by service page */
  messageTarget: string
  /** Current company id */
  companyId: number
  /** Current user id (email) */
  user: string
  /** Current user's UI language */
  language: string
  /** Current user's tenant id */
  tenantId: string
  /** Current branch id */
  branchId: string
}

export type ErrorResponse = {
  /** Command is the value that initially was sent in the message by the service page.
   * If either responsecmd was not set in request or the data sent from the request was missing/faulty, this value will be set to "error"
   */
  command: string
  /** Response data text if api call has failed */
  data: 'Unrecognized command'
}
