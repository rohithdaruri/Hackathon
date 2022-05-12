import React, { Component } from 'react';

export class Records extends Component {
    static displayName = Records.name;

    constructor(props) {
        super(props);
        this.state = { records: [], loading: true };
    }

    componentDidMount() {
        this.populateData();
    }

    static renderRecordsTable(forecasts) {
        return (
                <div class="table-responsive-sm">
                    <table className='table table-bordered table-sm' aria-labelledby="tabelLabel">
                        <thead>
                            <tr>
                                <th>Unique Id</th>
                                <th>Legal FirstName</th>
                                <th>Legal LastName</th>
                                <th>Prefered Name</th>
                                <th>Language</th>
                                <th>Phonetics</th>
                                <th>Type</th>
                                <th>Blob Path</th>
                            </tr>
                        </thead>
                        <tbody>
                            {forecasts.map(forecast =>
                                <tr key={forecast.id}>
                                    <td>{forecast.uniqueId}</td>
                                    <td>{forecast.legalFirstName}</td>
                                    <td>{forecast.legalLastName}</td>
                                    <td>{forecast.preferedName}</td>
                                    <td>{forecast.language}</td>
                                    <td>{forecast.phonetics}</td>
                                    <td>{forecast.processType}</td>
                                    <td>{forecast.blobPath}</td>
                                </tr>
                            )}
                        </tbody>
                    </table>
                </div>
        );
    }

    render() {
        let contents = this.state.loading
            ? <p><em>Loading...</em></p>
            : Records.renderRecordsTable(this.state.records);

        return (
            <div>
                <h1 id="tabelLabel" >Records</h1>
                {contents}
            </div>
        );
    }

    async populateData() {
        const response = await fetch('/api/TextToSpeech/Records');
        const data = await response.json();
        this.setState({ records: data, loading: false });
    }
}
