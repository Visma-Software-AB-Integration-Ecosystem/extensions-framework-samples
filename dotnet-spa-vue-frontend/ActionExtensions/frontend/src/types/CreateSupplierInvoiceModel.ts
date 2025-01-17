import type { ValueModel } from './ValueModel'
/** Base values to be used for creating a supplier invoice in Visma Net.
 * To find all the possible values for the properties, see the Visma Net API documentation.
 */
export interface CreateSupplierInvoiceModel {
  supplierNumber: ValueModel<string>
  supplierReference: ValueModel<string>
  invoiceLines: CreateSupplierInvoiceLineModel[]
}

export interface CreateSupplierInvoiceLineModel {
  lineNumber: ValueModel<number>
  inventoryNumber: ValueModel<string>
}
