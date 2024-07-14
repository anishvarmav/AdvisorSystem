export class Advisor {
    id?: number;
    name?: string;
    sin?: string;
    address?: string;
    phone?: string;
    healthStatus?: string;

    constructor(id?: number, name?: string, sin?: string, address?: string, phone?: string, healthStatus?: string) {
        this.id = id;
        this.name = name;
        this.sin = sin;
        this.address = address;
        this.phone = phone;
        this.healthStatus = healthStatus;
    }
}